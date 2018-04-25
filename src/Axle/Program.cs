// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Serilog;
    using Serilog.Events;

    public static class Program
    {
        public static int Main(string[] args)
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
                .AddCommandLine(args)
                .AddUserSecrets<Startup>()
                .Build();

            // LINK (Cameron): https://mitchelsellers.com/blogs/2017/10/09/real-world-aspnet-core-logging-configuration
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Async(a => a.Console())
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            var assembly = typeof(Program).Assembly;
            var title = assembly.Attribute<AssemblyTitleAttribute>(attribute => attribute.Title);
            var version = assembly.Attribute<AssemblyInformationalVersionAttribute>(attribute => attribute.InformationalVersion);
            var copyright = assembly.Attribute<AssemblyCopyrightAttribute>(attribute => attribute.Copyright);
            Log.Information($"{title} [{version}] {copyright}");
            Log.Information($"Running on: {RuntimeInformation.OSDescription}");

            Console.Title = $"{title} [{version}]";

            try
            {
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
            return WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(configuration)
                .UseStartup<Startup>()
                .UseSerilog()
                .UseUrls("http://+:5012")
                .Build();
        }
    }
}
