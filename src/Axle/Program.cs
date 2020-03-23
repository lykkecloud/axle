// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Axle.Settings;
    using Lykke.Snow.Common.Startup;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Serilog;

    public static class Program
    {
        private static readonly List<(string, string, string)> EnvironmentSecretConfig = new List<(string, string, string)>
        {
            /* secrets.json Key             // Environment Variable        // default value (optional) */
            ("Api-Authority",               "API_AUTHORITY",               null),
            ("Api-Name",                    "API_NAME",                    null),
            ("Api-Secret",                  "API_SECRET",                  null),
            ("ConnectionStrings:Redis",     "REDIS_CONNECTIONSTRING",      null),
            ("ConnectionStrings:RabbitMq",  "RABBITMQ_CONNECTIONSTRING",   null),
            ("mtCoreAccountsApiKey",        "MTCOREACCOUNTSAPIKEY",        string.Empty),
            ("chestApiKey",                 "CHEST_API_KEY",               string.Empty),
            ("Require-Https",               "REQUIRE_HTTPS",               "true"),
            ("Swagger-Client-Id",           "SWAGGER_CLIENT_ID",           "axle_api_swagger"),
            ("Validate-Issuer-Name",        "VALIDATE_ISSUER_NAME",        "false")
        };

        public static async Task<int> Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                Log.Fatal((Exception)e.ExceptionObject, "Host terminated unexpectedly");
                Log.CloseAndFlush();
            };

            // HACK (Cameron): Currently, there is no nice way to get a handle on IHostingEnvironment inside of Main() so we work around this...
            // LINK (Cameron): https://github.com/aspnet/KestrelHttpServer/issues/1334
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.Custom.json", optional: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .AddEnvironmentSecrets<Startup>(EnvironmentSecretConfig)
                .AddCommandLine(args)
                .Build();

            var assembly = typeof(Program).Assembly;
            var title = assembly.Attribute<AssemblyTitleAttribute>(attribute => attribute.Title);
            var version = assembly.Attribute<AssemblyInformationalVersionAttribute>(attribute => attribute.InformationalVersion);
            var copyright = assembly.Attribute<AssemblyCopyrightAttribute>(attribute => attribute.Copyright);

            // LINK (Cameron): https://mitchelsellers.com/blogs/2017/10/09/real-world-aspnet-core-logging-configuration
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithProperty("Application", title)
                .Enrich.WithProperty("Version", version)
                .Enrich.WithProperty("Environment", environmentName)
                .CreateLogger();

            Log.Information($"{title} [{version}] {copyright}");
            Log.Information($"Running on: {RuntimeInformation.OSDescription}");

            Console.Title = $"{title} [{version}]";
            try
            {
                configuration.ValidateEnvironmentSecrets(EnvironmentSecretConfig, Log.Logger);

                await configuration.ValidateSettings<AppSettings>();

                Log.Information($"Starting {title} web API");
                BuildWebHost(args, configuration).Run();
                Log.Information($"{title} web API stopped");
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IWebHost BuildWebHost(string[] args, IConfigurationRoot configuration)
        {
            return WebHost
                .CreateDefaultBuilder(args)
                .UseConfiguration(configuration)
                .ConfigureAppConfiguration(c =>
                {
                    c.AddEnvironmentSecrets<Startup>(EnvironmentSecretConfig);
                })
                .UseStartup<Startup>()
                .UseSerilog()
                .Build();
        }
    }
}
