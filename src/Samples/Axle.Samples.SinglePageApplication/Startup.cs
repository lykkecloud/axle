// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace SampleSinglePageApp
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConnections();

            services.AddSignalR(options =>
            {
                options.KeepAliveInterval = TimeSpan.FromSeconds(5);
            });

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


        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseCors("Everything");
        }
    }
}
