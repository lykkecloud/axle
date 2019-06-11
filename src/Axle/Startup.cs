// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Axle.Authorization;
    using Axle.Caches;
    using Axle.Configurators;
    using Axle.Constants;
    using Axle.Contracts;
    using Axle.Extensions;
    using Axle.HostedServices;
    using Axle.HttpClients;
    using Axle.Hubs;
    using Axle.Persistence;
    using Axle.Services;
    using Chest.Client.AutorestClient;
    using IdentityModel;
    using IdentityModel.Client;
    using IdentityServer4.AccessTokenValidation;
    using Lykke.HttpClientGenerator;
    using Lykke.Middlewares;
    using Lykke.Middlewares.Mappers;
    using Lykke.RabbitMqBroker.Publisher;
    using Lykke.RabbitMqBroker.Subscriber;
    using Lykke.Snow.Common.Startup;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;
    using NSwag.AspNetCore;
    using PermissionsManagement.Client;
    using PermissionsManagement.Client.Dto;
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
                    p.WithOrigins(this.configuration.GetSection("CorsOrigins").Get<string[]>())
                     .AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowCredentials();
                });
            });

            services
                .AddSignalR()
                .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerSettings.ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() };
                    options.PayloadSerializerSettings.Converters.Add(new StringEnumConverter());
                    options.PayloadSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            services.AddMvcCore()
                .AddJsonFormatters()
                .AddJsonOptions(
                    options =>
                    {
                        options.SerializerSettings.ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() };
                        options.SerializerSettings.Converters.Add(new StringEnumConverter());
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddApiExplorer()
                .AddAuthorization(
                    options =>
                    {
                        // add any authorization policy
                        options.AddPolicy(AuthorizationPolicies.System, policy => policy.RequireClaim(JwtClaimTypes.Scope, "axle_api:server"));
                        options.AddPolicy(AuthorizationPolicies.Mobile, policy => policy.AddRequirements(new MobileClientAndAccountOwnerRequirement()));
                        options.AddPolicy(PermissionsManagement.Client.Constants.AuthorizeUserPolicy, policy => policy.AddRequirements(new AuthorizeUserRequirement()));
                        options.AddPolicy(AuthorizationPolicies.AccountOwnerOrSupport, policy => policy.AddRequirements(new AccountOwnerOrSupportRequirement()));
                    });

            services.AddSwagger();

            var authority = this.configuration.GetValue<string>("Api-Authority");
            var apiName = this.configuration.GetValue<string>("Api-Name");
            var apiSecret = this.configuration.GetValue<string>("Api-Secret");
            var validateIssuerName = this.configuration.GetValue<bool>("Validate-Issuer-Name");
            var requireHttps = this.configuration.GetValue<bool>("Require-Https");

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

                        options.EnableCaching = this.configuration.GetValue("IntrospectionCache:Enabled", true);
                        options.CacheDuration = TimeSpan.FromSeconds(this.configuration.GetValue("IntrospectionCache:DurationInSeconds", 600));
                    });

            var connectionRepository = new InMemoryRepository<string, HubCallerContext>();

            services.AddSingleton<IRepository<string, HubCallerContext>>(connectionRepository);
            services.AddSingleton<IReadOnlyRepository<string, HubCallerContext>>(connectionRepository);

            var sessionTimeout = TimeSpan.FromSeconds(this.configuration.GetValue<int>("SessionConfig:TimeoutInSec", 300));

            services.AddSingleton<INotificationService, NotificationService>();

            services.AddSingleton<ISessionRepository, RedisSessionRepository>(x =>
                new RedisSessionRepository(
                    x.GetService<IConnectionMultiplexer>(),
                    sessionTimeout));
            services.AddSingleton<ISessionLifecycleService, SessionLifecycleService>(x =>
                new SessionLifecycleService(
                    x.GetService<ISessionRepository>(),
                    x.GetService<ITokenRevocationService>(),
                    x.GetService<INotificationService>(),
                    x.GetService<IActivityService>(),
                    x.GetService<ILogger<SessionLifecycleService>>(),
                    sessionTimeout));
            services.AddSingleton<IActivityService, ActivityService>();

            var rabbitMqSettings = this.configuration.GetSection("ActivityPublisherSettings").Get<RabbitMqSubscriptionSettings>().MakeDurable();
            rabbitMqSettings.ConnectionString = this.configuration["ConnectionStrings:RabbitMq"];

#pragma warning disable CS0618 // Type or member is obsolete
            services.AddSingleton(x => new RabbitMqPublisher<SessionActivity>(rabbitMqSettings)
                .DisableInMemoryQueuePersistence()
                .SetSerializer(new MessagePackMessageSerializer<SessionActivity>())
                .SetPublishStrategy(new DefaultFanoutPublishStrategy(rabbitMqSettings))
                .SetLogger(new LykkeLoggerAdapter<RabbitMqPublisher<SessionActivity>>(x.GetService<ILogger<RabbitMqPublisher<SessionActivity>>>()))
                .PublishSynchronously());
#pragma warning restore CS0618 // Type or member is obsolete

            services.AddSingleton(provider => new DiscoveryClient(authority)
            {
                Policy = new DiscoveryPolicy
                {
                    ValidateIssuerName = validateIssuerName,
                    RequireHttps = requireHttps
                }
            });

            services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(this.configuration.GetValue<string>("ConnectionStrings:Redis")));

            services.AddSingleton<DiscoveryCache>();
            services.AddSingleton<ITokenRevocationService, BouncerService>();

            services.AddSingleton<IHttpStatusCodeMapper, DefaultHttpStatusCodeMapper>();
            services.AddSingleton<ILogLevelMapper, DefaultLogLevelMapper>();
            services.AddSingleton<AuditSettings>(this.configuration.GetSection("AuditSettings")?.Get<AuditSettings>() ?? new AuditSettings());

            services.AddMtCoreDalRepositories(
                this.configuration.GetValue<string>("mtCoreAccountsMgmtServiceUrl"),
                this.configuration.GetValue<string>("mtCoreAccountsApiKey"));

            services.AddSingleton<IChestClient>(provider => new ChestClient(new Uri(this.configuration.GetValue<string>("chestUrl")), new ExceptionTextWithServiceNameEnricher("Chest API")));

            services.AddSingleton<IAccountsService, AccountsService>();

            services.AddSingleton<IEnumerable<SecurityGroup>>(this.configuration.GetSection("SecurityGroups").Get<IEnumerable<SecurityGroup>>());
            services.AddSingleton<IUserRoleToPermissionsTransformer, UserRoleToPermissionsTransformer>();
            services.AddSingleton<IUserPermissionsClient, FakeUserPermissionsRepository>();
            services.AddSingleton<IClaimsTransformation, ClaimsTransformation>();
            services.AddSingleton<IAuthorizationHandler, AuthorizeUserHandler>();
            services.AddSingleton<IAuthorizationHandler, AccountOwnerOrSupportHandler>();
            services.AddSingleton<IAuthorizationHandler, MobileClientAndAccountOwnerHandler>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IAccountsCache, AccountsCache>();

            services.AddMemoryCache(o => o.ExpirationScanFrequency = TimeSpan.FromMinutes(1));
            services.AddDistributedMemoryCache(
                options => options.ExpirationScanFrequency = TimeSpan.FromSeconds(
                    this.configuration.GetValue("IntrospectionCache:ExpirationScanFrequencyInSeconds", 60)));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }

            app.UseMiddleware<LogHandlerMiddleware>();
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseCors("AllowCors");

            app.UseAuthentication();
            app.UseMiddleware<AuditHandlerMiddleware>();

            app.UseSignalR(routes =>
            {
                routes.MapHub<SessionHub>(SessionHub.Name);
            });

            // Enable the Swagger UI middleware and the Swagger generator.
            var assembly = typeof(Startup).GetTypeInfo().Assembly;
            app.UseSwaggerUi3WithApiExplorer(
                SwaggerConfigurator.Configure(
                        assembly,
                        this.configuration.GetValue<string>("Api-Authority"),
                        this.configuration.GetValue<string>("Api-Name"),
                        this.configuration.GetValue<string>("Swagger-Client-Id")));

            app.UseMvc();
        }
    }
}
