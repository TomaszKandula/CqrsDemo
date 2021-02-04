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
using CqrsDemo.Logger;
using CqrsDemo.Database;
using CqrsDemo.Exceptions;
using CqrsDemo.Services.Commands;
using CqrsDemo.Services.Authentication;
using CqrsDemo.Handlers.Queries.GetParkingInfo;
using CqrsDemo.Handlers.Queries.GetAllParkingInfo;
using CqrsDemo.Handlers.Queries.GetTotalAvailablePlaces;
using CqrsDemo.Handlers.Queries.GetRandomAvailablePlace;
using CqrsDemo.Handlers.Commands.OpenParking;
using CqrsDemo.Handlers.Commands.CloseParking;
using CqrsDemo.Handlers.Commands.CreateParking;
using CqrsDemo.Handlers.Commands.TakeParkingPlace;
using CqrsDemo.Handlers.Commands.LeaveParkingPlace;
using MediatR;

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
            
            AServices.AddDbContext<MainDbContext>(AOptions =>
            {
                AOptions.UseSqlServer(Configuration.GetConnectionString("DbConnect"),
                AAddOptions => AAddOptions.EnableRetryOnFailure());
            });
            
            AServices.AddSingleton<IAppLogger, AppLogger>();
            AServices.AddScoped<IAuthentication, Authentication>();
            AServices.AddScoped<ICommands, Commands>();

            AServices.AddTransient<IRequestHandler<GetParkingInfoQuery, GetParkingInfoQueryResult>, GetParkingInfoQueryHandler>();
            AServices.AddTransient<IRequestHandler<GetAllParkingInfoQuery, IEnumerable<GetAllParkingInfoQueryResult>>, GetAllParkingInfoQueryHandler>();
            AServices.AddTransient<IRequestHandler<GetRandomAvailablePlaceQuery, GetRandomAvailablePlaceQueryResult>, GetRandomAvailablePlaceQueryHandler>();
            AServices.AddTransient<IRequestHandler<GetTotalAvailablePlacesQuery, GetTotalAvailablePlacesQueryResult>, GetTotalAvailablePlacesQueryHandler>();

            AServices.AddTransient<IRequestHandler<CloseParkingCommand, Unit>, CloseParkingCommandHandler>();
            AServices.AddTransient<IRequestHandler<CreateParkingCommand, Unit>, CreateParkingCommandHandler>();
            AServices.AddTransient<IRequestHandler<LeaveParkingPlaceCommand, Unit>, LeaveParkingPlaceCommandHandler>();
            AServices.AddTransient<IRequestHandler<OpenParkingCommand, Unit>, OpenParkingCommandHandler>();
            AServices.AddTransient<IRequestHandler<TakeParkingPlaceCommand, Unit>, TakeParkingPlaceCommandHandler>();

            AServices.AddResponseCompression(AOptions => { AOptions.Providers.Add<GzipCompressionProvider>(); });

            AServices.AddSwaggerGen(AOption =>
            {
                AOption.SwaggerDoc("v1", new OpenApiInfo { Title = "CqrsDemo Api", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder AApp, IWebHostEnvironment AEnv)
        {
            AApp.UseExceptionHandler(ExceptionHandler.Handle);
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
