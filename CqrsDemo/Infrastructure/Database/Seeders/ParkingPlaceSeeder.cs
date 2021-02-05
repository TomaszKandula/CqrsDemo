using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CqrsDemo.Infrastructure.Domain.Entities;

namespace CqrsDemo.Infrastructure.Database.Seeders
{
    public class ParkingPlaceSeeder : IMainDbContextSeeder
    {
        public void Seed(ModelBuilder AModelBuilder)
        {
            new List<ParkingPlace>
            {
                new ParkingPlace
                {
                    ParkingName = "Poznan Plaza",
                    Number = 1,
                    IsFree = true,
                    UserId = null,
                    ParkingNameNavigation = new Parking
                    {
                        Name = "Poznan Plaza",
                        IsOpened = true
                    }
                },
                new ParkingPlace
                {
                    ParkingName = "Poznan Plaza",
                    Number = 2,
                    IsFree = true,
                    UserId = null,
                    ParkingNameNavigation = new Parking
                    {
                        Name = "Poznan Plaza",
                        IsOpened = true
                    }
                },
                new ParkingPlace
                {
                    ParkingName = "Parking-786359",
                    Number = 3,
                    IsFree = true,
                    UserId = null,
                    ParkingNameNavigation = new Parking
                    {
                        Name = "Parking-786359",
                        IsOpened = true
                    }
                },
                new ParkingPlace
                {
                    ParkingName = "Parking-786359",
                    Number = 4,
                    IsFree = false,
                    UserId = null,
                    ParkingNameNavigation = new Parking
                    {
                        Name = "Parking-786359",
                        IsOpened = true
                    }
                }
            };
        }
    }
}
