using System.Reflection;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.ResponseCompression;
using CqrsDemo.Database;
using CqrsDemo.AppLogger;
using CqrsDemo.Models.Responses;
using CqrsDemo.Handlers.Queries;
using CqrsDemo.Services.Commands;
using CqrsDemo.Handlers.Commands;
using CqrsDemo.Services.Authentication;
using CqrsDemo.Handlers.Queries.Models;
using CqrsDemo.Handlers.Commands.Models;
using MediatR;
using AutoMapper;

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
            AServices.AddMediatR(Assembly.GetExecutingAssembly());
            AServices.AddAutoMapper(typeof(Startup));
            
            AServices.AddDbContext<MainDbContext>(AOptions =>
            {
                AOptions.UseSqlServer(Configuration.GetConnectionString("DbConnect"),
                AAddOptions => AAddOptions.EnableRetryOnFailure());
            });
            
            AServices.AddSingleton<IAppLogger, AppLogger.AppLogger>();
            AServices.AddScoped<IAuthentication, Authentication>();
            AServices.AddScoped<ICommands, Commands>();

            AServices.AddTransient<IRequestHandler<GetParkingInfo, ParkingInfo>, HandleParkingInfo>();
            AServices.AddTransient<IRequestHandler<GetAllParkingInfo, IEnumerable<ParkingInfo>>, HandleAllParkingInfo>();
            AServices.AddTransient<IRequestHandler<GetRandomAvailablePlace, ParkingPlaceInfo>, HandleRandomAvailablePlace>();
            AServices.AddTransient<IRequestHandler<GetTotalAvailablePlaces, AvailablePlaceInfo>, HandleTotalAvailablePlaces>();

            AServices.AddTransient<IRequestHandler<CloseParking, CommandResponse>, HandleCloseParking>();
            AServices.AddTransient<IRequestHandler<CreateParking, CommandResponse>, HandleCreateParking>();
            AServices.AddTransient<IRequestHandler<LeaveParkingPlace, CommandResponse>, HandleLeaveParkingPlace>();
            AServices.AddTransient<IRequestHandler<OpenParking, CommandResponse>, HandleOpenParking>();
            AServices.AddTransient<IRequestHandler<TakeParkingPlace, CommandResponse>, HandleTakeParkingPlace>();

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
