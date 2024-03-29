﻿using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CqrsDemo.Infrastructure.Domain.Entities;

namespace CqrsDemo.Infrastructure.Database.Seeders
{
    public class ParkingSeeder : IMainDbContextSeeder
    {
        public void Seed(ModelBuilder AModelBuilder)
            => AModelBuilder.Entity<Parking>().HasData(CreateParkingList());

        private static IEnumerable<Parking> CreateParkingList()
        {
            return new List<Parking>
            {
                new Parking
                {
                    Name = "Poznan Plaza",
                    IsOpened = false
                },
                new Parking
                {
                    Name = "Parking-786359",
                    IsOpened = true,
                }
            };
        }
    }
}
