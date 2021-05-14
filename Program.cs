using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SalesforceExternalClientAppDemo.ConsoleApp.Application;
using SalesforceExternalClientAppDemo.ConsoleApp.Authentication;
using SalesforceExternalClientAppDemo.ConsoleApp.Settings;
using SalesforceExternalClientAppDemo.ConsoleApp.Validators;
using Serilog;
using Serilog.Formatting.Json;
using System;
using System.Linq;

namespace SalesforceExternalClientAppDemo.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var configuration = BuildConfig();
            SetupLogger(configuration);

            var salesforceSettings = GetSalesforceSettings(configuration);
            if (!SalesforceSettingsAreValid(salesforceSettings))
            {
                Log.Logger.Fatal($"Validation errors encountered in 'SalesforceSettings' configuration.  Aborting.");
                return;
            }

            Log.Logger.Information("Application Starting");

            var hostBuilder = Host.CreateDefaultBuilder();

            var accessToken = DoSalesforceAuthentication(configuration);
            if (accessToken == null)
            {
                Log.Logger.Fatal($"Cannot continue without a valid access token. Aborting.");
                return;
            }

            var host = hostBuilder.ConfigureServices((context, services) =>
            {
                services.AddTransient<IStartup, Startup>();
                services.AddTransient<IHelloWorldConnector, HelloWorldConnector>();
                services.AddTransient<IClaimsConnector, ClaimsConnector>();

                SalesforceSettings salesforceSettings = GetSalesforceSettings(configuration);
                salesforceSettings.AccessToken = accessToken;

                services.AddSingleton(salesforceSettings);
                services.AddHttpClient();
            })
                .UseSerilog()
                .Build();

            var startup = ActivatorUtilities.CreateInstance<Startup>(host.Services);
            startup.Run().Wait();
        }

        private static bool SalesforceSettingsAreValid(SalesforceSettings salesforceSettings)
        {
            var settingsErrors = salesforceSettings.ValidationErrors().ToArray();
            if (settingsErrors.Any())
            {
                var errorString = string.Join(",", settingsErrors);
                Log.Logger.Error("Errors found in salesforce settings config: {@errorString}, salesforceSettings: {@salesforceSettings}",
                    errorString, salesforceSettings);
                return false;
            }

            // TODO
            // Check to see if the private key file is there, accessible, and an actual private key

            return true;
        }

        private static SalesforceSettings GetSalesforceSettings(IConfigurationRoot configuration)
        {
            var salesforceSettingsSection = configuration.GetSection("SalesforceSettings");
            var salesforceSettings = salesforceSettingsSection.Get<SalesforceSettings>();
            return salesforceSettings;
        }

        private static void SetupLogger(IConfigurationRoot config)
        {
            Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(config)
                            .Enrich.FromLogContext()
                            .WriteTo.Console()
                            .WriteTo.File(formatter: new JsonFormatter(), path: @"logs\salesforce-external-client-app-demo.log",
                                rollingInterval: RollingInterval.Hour)
                            .CreateLogger();
        }

        private static IConfigurationRoot BuildConfig()
        {
            var builder = new ConfigurationBuilder();
            var config = builder.SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                optional: true, reloadOnChange: true)
                //.AddUserSecrets<Program>()
                .AddEnvironmentVariables()
                .Build();

            return config;
        }

        private static string DoSalesforceAuthentication(IConfigurationRoot configuration)
        {
            var salesforceSettings = GetSalesforceSettings(configuration);
            var authProvider = new AuthenticationHelper(salesforceSettings);

            try
            {
                return authProvider.DoAuthentication();
            }
            catch (Exception e)
            {
                Log.Logger.Fatal("Error occurred during authentication.  Aborting. Details: {@e}", e);
                return null;
            }
        }
    }
}