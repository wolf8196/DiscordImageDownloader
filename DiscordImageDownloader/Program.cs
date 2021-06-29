using System;
using System.IO;
using System.Reflection;
using DiscordImageDownloader.Core;
using DiscordImageDownloader.Discord;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DiscordImageDownloader
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            return Host
                .CreateDefaultBuilder(args)
                .UseSerilog((hostBuilder, loggerConfig) =>
                {
                    loggerConfig.ReadFrom.Configuration(hostBuilder.Configuration).Enrich.WithProperty("App", "DiscordImageDownloader");
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var workerSettings = hostContext.Configuration.GetSection(nameof(WorkerSettings)).Get<WorkerSettings>();
                    services.AddSingleton(workerSettings);

                    services.AddDiscordWorkers(hostContext.Configuration);
                })
                .UseWindowsService();
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Fatal(
                (Exception)e.ExceptionObject,
                "Unhandled exception caught. Runtime is terminating : {IsTerminating}.",
                e.IsTerminating);

            Log.CloseAndFlush();
        }
    }
}