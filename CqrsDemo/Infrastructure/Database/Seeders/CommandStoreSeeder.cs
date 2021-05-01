using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CqrsDemo.Infrastructure.Domain.Entities;

namespace CqrsDemo.Infrastructure.Database.Seeders
{
    public class CommandStoreSeeder : IMainDbContextSeeder
    {
        public void Seed(ModelBuilder AModelBuilder)
            => AModelBuilder.Entity<CommandStore>().HasData(CreateCommandStores());

        private static IEnumerable<CommandStore> CreateCommandStores()
        {
            return new List<CommandStore>
            {
                new CommandStore
                {
                    Id = 1,
                    Type = "CreateParking",
                    Data = "{\"ParkingName\":\"Poznan Plaza\",\"Capacity\":2}",
                    CreatedAt = DateTime.Parse("2020-12-04 20:28:03"),
                    UserId = "30fb43bf-9689-4a16-b41f-75775d11a02f"
                },
                new CommandStore
                {
                    Id = 2,
                    Type = "CreateParking",
                    Data = "{\"ParkingName\":\"Parking-786359\",\"Capacity\":2}",
                    CreatedAt = DateTime.Parse("2020-12-04 20:28:03"),
                    UserId = "30fb43bf-9689-4a16-b41f-75775d11a02f"
                }
            };
        }
    }
}
