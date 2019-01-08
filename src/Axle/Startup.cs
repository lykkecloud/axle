// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle
{
    using System.Security.Claims;
    using Axle.Hubs;
    using Axle.Persistence;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
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
            var connectionRepository = new InMemoryRepository<string, ClaimsPrincipal>();

            services.AddSingleton<IRepository<string, ClaimsPrincipal>>(connectionRepository);
            services.AddSingleton<IReadOnlyRepository<string, ClaimsPrincipal>>(connectionRepository);

            services.AddSingleton<ISessionRepository, InMemorySessionRepository>();
            services.AddTransient<SessionHubMethods<SessionHub>>();

            services.AddCors(o =>
             {
                 o.AddPolicy("Everything", p =>
                 {
                     p.AllowAnyHeader()
                         .AllowAnyMethod()
                         .AllowAnyOrigin()
                         .AllowCredentials();
                 });
             });

            services.AddSignalR();

            services.AddMvcCore()
                .AddJsonFormatters();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }

            app.UseCors("Everything");
            app.UseMvc();
            app.UseSignalR(routes =>
            {
                routes.MapHub<SessionHub>(SessionHub.Name);
            });
        }
    }
}
