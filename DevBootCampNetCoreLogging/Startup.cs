using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DevBootCampNetCoreLogging
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            //here we are injecting the configuration providers that were registered in Program.cs.
            // If you don't explicitly register configuration providers in Progam.cs, you can still use the below line and 
            //it will use the defaults, which are  appsettings.json (for Production) and appsettings.Development.json (for Development).
            //coincidentally we are choosing to use these same providers anyway

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation("In the Startup Configure method at " + DateTime.UtcNow);
            if (env.IsDevelopment())
            {
                //throw new Exception("fake development exception");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //throw new Exception("fake production exception");
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            /*
            try
            {
                throw new Exception("fake startup configure method exception");
            }
            catch (Exception)
            {
                logger.LogInformation("Fake startup configure method exception was handled in the Main method at " + DateTime.UtcNow);
            }
            */

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
