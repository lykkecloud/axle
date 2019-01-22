// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle
{
    using Axle.Authorization;
    using Axle.Hubs;
    using Axle.Persistence;
    using Axle.Services;
    using IdentityModel.Client;
    using IdentityServer4.AccessTokenValidation;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

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
                    p.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                        .AllowCredentials();
                });
            });

            services.AddSignalR();

            services.AddMvcCore()
                .AddJsonFormatters()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddAuthorization(
                    options =>
                    {
                        // add any authorization policy
                    });

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
                    });

            var connectionRepository = new InMemoryRepository<string, HubCallerContext>();

            services.AddSingleton<IRepository<string, HubCallerContext>>(connectionRepository);
            services.AddSingleton<IReadOnlyRepository<string, HubCallerContext>>(connectionRepository);

            services.AddSingleton<ISessionRepository, InMemorySessionRepository>();
            services.AddTransient<SessionHubMethods<SessionHub>>();

            services.AddSingleton(provider => new DiscoveryClient(authority)
            {
                Policy = new DiscoveryPolicy
                {
                    ValidateIssuerName = validateIssuerName,
                    RequireHttps = requireHttps
                }
            });

            services.AddSingleton<DiscoveryCache>();
            services.AddSingleton<ITokenRevocationService, BouncerService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }

            app.UseCors("AllowCors");

            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                routes.MapHub<SessionHub>(SessionHub.Name);
            });

            app.UseMvc();
        }
    }
}
