using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CqrsDemo
{
    public class Startup
    {

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration AConfiguration)
        {
            Configuration = AConfiguration;
        }

        public void ConfigureServices(IServiceCollection AServices)
        {
            AServices.AddControllers();
        }

        public void Configure(IApplicationBuilder AApp, IWebHostEnvironment AEnv)
        {

            if (AEnv.IsDevelopment())
            {
                AApp.UseDeveloperExceptionPage();
            }

            AApp.UseHttpsRedirection();
            AApp.UseRouting();
            AApp.UseAuthorization();
            AApp.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
