using Microsoft.EntityFrameworkCore;
using CqrsDemo.Infrastructure.Domain.Entities;

namespace CqrsDemo.Infrastructure.Database.Seeders
{
    public class ParkingPlaceSeeder : IMainDbContextSeeder
    {
        public void Seed(ModelBuilder AModelBuilder)
        {
            AModelBuilder.Entity<ParkingPlace>().HasData(
                new ParkingPlace
                {
                    ParkingName = "Poznan Plaza",
                    Number = 1,
                    IsFree = true,
                    UserId = null
                },
                new ParkingPlace
                {
                    ParkingName = "Poznan Plaza",
                    Number = 2,
                    IsFree = true,
                    UserId = null
                },
                new ParkingPlace
                {
                    ParkingName = "Parking-786359",
                    Number = 3,
                    IsFree = true,
                    UserId = null
                },
                new ParkingPlace
                {
                    ParkingName = "Parking-786359",
                    Number = 4,
                    IsFree = false,
                    UserId = null
                });
        }
    }
}
