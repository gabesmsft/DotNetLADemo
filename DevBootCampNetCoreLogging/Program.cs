using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Microsoft.Extensions.DependencyInjection;

//add this in order to use App Service logging
using Microsoft.Extensions.Logging.AzureAppServices;

//add this in order to send logs to App Insights (without using App Insights SDK logging)
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Extensions.Hosting;

namespace DevBootCampNetCoreLogging
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // CreateWebHostBuilder(args).Build().Run();
            var host = CreateWebHostBuilder(args).Build();
           var logger = host.Services.GetRequiredService<ILogger<Program>>();
           logger.LogInformation("In the Main method at " + DateTime.UtcNow);

            /*
            try
            {
               throw new Exception("fake main method exception");
            }
            catch(Exception)
            {
                logger.LogInformation("Fake exception was handled in the Main method at " + DateTime.UtcNow);
            }
            */

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)

            //You can actually leave out this .ConfigureAppConfiguration section,
            //because the application will add appsettings.json (for Production) and appsettings.Development.json (for Development)
            //to the list of configuration providers by default even if you don't implement this section.
            //The purpose of this sample ConfigureAppConfiguration section is just to demonstrate how you *can"
            // customize the configuration providers if you choose to.

            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                //config.Sources.Clear();

                var env = hostingContext.HostingEnvironment;
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                      .AddJsonFile($"appsettings.{env.EnvironmentName}.json",
                                     optional: true, reloadOnChange: true)
                      .AddEnvironmentVariables();
                
            })

            .ConfigureLogging((hostingContext, logging) =>
            {
                //by default, CreateDefaultBuilder registers Console, Debug, EventSource, and (if Windows) EventLog. ClearProviders removes these registrations
                logging.ClearProviders();

                logging.AddConsole();

                logging.AddAzureWebAppDiagnostics();

                var appInsightKey = hostingContext.Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];
                if (appInsightKey != null && appInsightKey != "")
                {
                    logging.AddApplicationInsights(appInsightKey);
                }
            })
            .UseStartup<Startup>();

        //Note: Logging during host construction isn't directly supported. However, a separate logger can be used. 
        // See the following for an example of how to do this:
        //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-3.1#create-logs-in-the-program-class
    }
}
