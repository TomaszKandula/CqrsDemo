using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.ResponseCompression;
using CqrsDemo.Handlers;
using CqrsDemo.Database;
using CqrsDemo.AppLogger;
using CqrsDemo.Services.Commands;
using CqrsDemo.Services.Authentication;

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
            AServices.AddDbContext<MainDbContext>(AOptions =>
            {
                AOptions.UseSqlServer(Configuration.GetConnectionString("DbConnect"),
                AAddOptions => AAddOptions.EnableRetryOnFailure());
            });
            AServices.AddSingleton<IAppLogger, AppLogger.AppLogger>();
            AServices.AddScoped<IAuthentication, Authentication>();
            AServices.AddScoped<ICommands, Commands>();
            AServices.AddScoped<IHandlerContext, HandlerContext>();

            AServices.AddResponseCompression(AOptions => { AOptions.Providers.Add<GzipCompressionProvider>(); });

            AServices.AddSwaggerGen(AOption =>
            {
                AOption.SwaggerDoc("v1", new OpenApiInfo { Title = "CqrsDemo Api", Version = "v1" });
            });

        }

        public void Configure(IApplicationBuilder AApp, IWebHostEnvironment AEnv)
        {

            AApp.UseResponseCompression();
            
            if (AEnv.IsDevelopment())
            {
                AApp.UseDeveloperExceptionPage();
            }

            AApp.UseSwagger();
            AApp.UseSwaggerUI(AOption =>
            {
                AOption.SwaggerEndpoint("/swagger/v1/swagger.json", "CqrsDemo Api version 1");
            });
            
            AApp.UseHttpsRedirection();
            AApp.UseRouting();
            AApp.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
