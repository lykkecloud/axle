// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle
{
    using Axle.Hubs;
    using Axle.Persistence;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
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
            var connectionRepository = new InMemoryRepository<string, HubCallerContext>();

            services.AddSingleton<IRepository<string, HubCallerContext>>(connectionRepository);
            services.AddSingleton<IReadOnlyRepository<string, HubCallerContext>>(connectionRepository);

            services.AddSingleton<ISessionRepository, InMemorySessionRepository>();
            services.AddTransient<SessionHubMethods<SessionHub>>();

            services.AddMvcCore()
                .AddJsonFormatters();

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

            app.UseSignalR(routes =>
            {
                routes.MapHub<SessionHub>(SessionHub.Name);
            });

            app.UseMvc();
        }
    }
}
