// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Axle
{
    using System;
    using Authorization;
    using Caches;
    using Constants;
    using Contracts;
    using Extensions;
    using HostedServices;
    using Hubs;
    using Persistence;
    using Services;
    using Settings;
    using Chest.Client.Extensions;
    using IdentityModel;
    using IdentityModel.Client;
    using IdentityServer4.AccessTokenValidation;
    using Lykke.Middlewares;
    using Lykke.Middlewares.Mappers;
    using Lykke.RabbitMqBroker.Publisher;
    using Lykke.RabbitMqBroker.Subscriber;
    using Lykke.Snow.Common.Startup;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;
    using PermissionsManagement.Client;
    using PermissionsManagement.Client.Handlers;
    using StackExchange.Redis;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o =>
            {
                o.AddPolicy("AllowCors", p =>
                {
                    p.WithOrigins(configuration.GetSection("CorsOrigins").Get<string[]>())
                     .AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowCredentials();
                });
            });

            services
                .AddSignalR()
                .AddNewtonsoftJsonProtocol(options =>
                {
                    options.PayloadSerializerSettings.ContractResolver = new DefaultContractResolver
                        {NamingStrategy = new CamelCaseNamingStrategy()};
                    options.PayloadSerializerSettings.Converters.Add(new StringEnumConverter());
                    options.PayloadSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            services.AddMvcCore()
                .AddNewtonsoftJson(
                    options =>
                    {
                        options.SerializerSettings.ContractResolver = new DefaultContractResolver
                            {NamingStrategy = new CamelCaseNamingStrategy()};
                        options.SerializerSettings.Converters.Add(new StringEnumConverter());
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    })
                .AddApiExplorer()
                .AddAuthorization(
                    options =>
                    {
                        // add any authorization policy
                        options.AddPolicy(AuthorizationPolicies.System,
                            policy => policy.RequireClaim(JwtClaimTypes.Scope, "axle_api:server"));
                        options.AddPolicy(AuthorizationPolicies.Mobile,
                            policy => policy.AddRequirements(new MobileClientAndAccountOwnerRequirement()));
                        options.AddPolicy(PermissionsManagement.Client.Constants.AuthorizeUserPolicy,
                            policy => policy.AddRequirements(new AuthorizeUserRequirement()));
                        options.AddPolicy(AuthorizationPolicies.AccountOwnerOrSupport,
                            policy => policy.AddRequirements(new AccountOwnerOrSupportRequirement()));
                    });

            var authority = configuration.GetValue<string>("Api-Authority");
            var apiName = configuration.GetValue<string>("Api-Name");
            var apiSecret = configuration.GetValue<string>("Api-Secret");
            var validateIssuerName = configuration.GetValue<bool>("Validate-Issuer-Name");
            var requireHttps = configuration.GetValue<bool>("Require-Https");
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = apiName, Version = "v1"});
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{authority}/connect/authorize", UriKind.Absolute),
                            Scopes =
                            {
                                {apiName, "CFD Platform (Nova)"},
                                {$"{apiName}:server", "CFD Platform (Nova) server side methods"},
                                {$"{apiName}:mobile", "CFD Platform (Nova) mobile side methods"}
                            }
                        }
                    }
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "oauth2",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new[] {apiName}
                    }
                });
            });

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                })
                .AddIdentityServerAuthentication(
                    IdentityServerAuthenticationDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.Authority = authority;
                        options.ApiName = apiName;
                        options.ApiSecret = apiSecret;

                        // NOTE (Cameron): This is only used because we're performing HTTPS termination at the proxy.
                        options.RequireHttpsMetadata = requireHttps;
                        options.IntrospectionDiscoveryPolicy.RequireHttps = requireHttps;
                        options.IntrospectionDiscoveryPolicy.ValidateIssuerName = validateIssuerName;

                        options.TokenRetriever = BearerTokenRetriever.FromHeaderAndQueryString;

                        options.EnableCaching = configuration.GetValue("IntrospectionCache:Enabled", true);
                        options.CacheDuration = TimeSpan.FromSeconds(configuration.GetValue("IntrospectionCache:DurationInSeconds", 600));
                    });

            var connectionRepository = new InMemoryRepository<string, HubCallerContext>();

            services.AddSingleton<IRepository<string, HubCallerContext>>(connectionRepository);
            services.AddSingleton<IRepository<string, int>>(new InMemoryRepository<string, int>());
            services.AddSingleton<IReadOnlyRepository<string, HubCallerContext>>(connectionRepository);

            var sessionSettings = configuration.GetSection("SessionConfig").Get<SessionSettings>() ?? new SessionSettings();

            services.AddSingleton(sessionSettings);
            services.AddSingleton<INotificationService, NotificationService>();

            services.AddSingleton<ISessionRepository, RedisSessionRepository>(x =>
                new RedisSessionRepository(
                    x.GetService<IConnectionMultiplexer>(),
                    sessionSettings.Timeout,
                    x.GetService<ILogger<RedisSessionRepository>>()));
            services.AddSingleton<ISessionService, SessionService>();
            services.AddSingleton<IHubConnectionService, HubConnectionService>();
            services.AddSingleton<IActivityService, ActivityService>();

            var rabbitMqSettings = configuration.GetSection("ActivityPublisherSettings").Get<RabbitMqSubscriptionSettings>().MakeDurable();
            rabbitMqSettings.ConnectionString = configuration["ConnectionStrings:RabbitMq"];
            
            services.AddSingleton(x => new RabbitMqPublisher<SessionActivity>(rabbitMqSettings)
                .DisableInMemoryQueuePersistence()
                .SetSerializer(new MessagePackMessageSerializer<SessionActivity>())
                .SetPublishStrategy(new DefaultFanoutPublishStrategy(rabbitMqSettings))
                .SetLogger(new LykkeLoggerAdapter<RabbitMqPublisher<SessionActivity>>(x.GetService<ILogger<RabbitMqPublisher<SessionActivity>>>()))
                .PublishSynchronously());

            services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(configuration.GetValue<string>("ConnectionStrings:Redis")));

            services.AddSingleton<IDiscoveryCache, DiscoveryCache>(p => new DiscoveryCache(authority,
                new DiscoveryPolicy {RequireHttps = requireHttps, ValidateIssuerName = validateIssuerName}));
            services.AddSingleton<ITokenRevocationService, BouncerService>();

            services.AddSingleton<IHttpStatusCodeMapper, DefaultHttpStatusCodeMapper>();
            services.AddSingleton<ILogLevelMapper, DefaultLogLevelMapper>();
            
            // AuditSettings registration for Lykke.Middlewares.AuditHandlerMiddleware
            services.AddSingleton(configuration.GetSection("AuditSettings")?.Get<AuditSettings>() ?? new AuditSettings());

            services.AddMtCoreDalRepositories(
                configuration.GetValue<string>("mtCoreAccountsMgmtServiceUrl"),
                configuration.GetValue<string>("mtCoreAccountsApiKey"));

            services.AddChestClient(
                configuration.GetValue<string>("chestUrl"),
                configuration.GetValue<string>("chestApiKey"));

            services.AddSingleton<IAccountsService, AccountsService>();
            services.AddSingleton<IUserRoleToPermissionsTransformer, UserRoleToPermissionsTransformer>();
            services.AddSingleton<IUserPermissionsClient, FakeUserPermissionsRepository>();
            services.AddSingleton<IClaimsTransformation, ClaimsTransformation>();
            services.AddSingleton<IAuthorizationHandler, AuthorizeUserHandler>();
            services.AddSingleton<IAuthorizationHandler, AccountOwnerOrSupportHandler>();
            services.AddSingleton<IAuthorizationHandler, MobileClientAndAccountOwnerHandler>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IAccountsCache, AccountsCache>();

            services.AddHostedService<SessionExpirationService>();
            services.AddHostedService<SessionTerminationListener>();
            services.AddHostedService<OtherTabTerminationListener>();

            services.AddMemoryCache(o => o.ExpirationScanFrequency = TimeSpan.FromMinutes(1));
            services.AddDistributedMemoryCache(
                options => options.ExpirationScanFrequency = TimeSpan.FromSeconds(
                    configuration.GetValue("IntrospectionCache:ExpirationScanFrequencyInSeconds", 60)));

            services.AddHttpClient();
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }

            app.UseMiddleware<LogHandlerMiddleware>();
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseMiddleware<AuditHandlerMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                options.OAuthClientId(configuration.GetValue<string>("Swagger-Client-Id"));
                options.OAuthAppName(configuration.GetValue<string>("Api-Name"));
            });

            app.UseRouting();
            app.UseCors("AllowCors");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SessionHub>(SessionHub.Name);
                endpoints.MapControllers();
            });
        }
    }
}
