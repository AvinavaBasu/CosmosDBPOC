using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using WebApplication1.Model;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //private static readonly string EndpointUri = ConfigurationManager.AppSettings["EndPointUrl"];

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();
            CreateJsonFiles();
        }

        private async static void CreateJsonFiles()
        {
            for (int i = 1; i <= 100000; i++)
            {
                var employee = new Employees()
                {
                    EmployeeId = i,
                    FirstName = $"Avinava_{i}",
                    LastName = $"Basu_{i}",
                    Designation = "Developer"
                };
                
                var path = $@"D:\Project\Azure_cosmosdb_json\DummyJsons\Employee_{i}.json";

                if (File.Exists(path))
                {
                    File.Delete(path);
                    await WriteTofIles(path,employee);
                }
                else
                {
                   await WriteTofIles(path,employee);
                }
            }
        }

        private static async Task WriteTofIles(string path, Employees employee)
        {
            var result = JsonConvert.SerializeObject(employee);
            using (var tw = new StreamWriter(path, true))
            {
                tw.WriteLine(result.ToString());
                tw.Close();
            }
        }

        private void ExecuteSimpleQuery(string databaseName,string collection)
        {
            //var EndpointUri= Configuration.GetConnectionString
            //FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
            //IQueryable<Employees> employeeQuery = 
        }
    }
}
