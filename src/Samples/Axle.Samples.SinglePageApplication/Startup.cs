// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Axle.Samples.SinglePageApplication
{
    public class Startup
    {
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
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseFileServer();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowCors");
        }
    }
}
