using Microsoft.EntityFrameworkCore;
using CqrsDemo.Infrastructure.Domain.Entities;

namespace CqrsDemo.Infrastructure.Database.Seeders
{
    public class ParkingSeeder : IMainDbContextSeeder
    {
        public void Seed(ModelBuilder AModelBuilder)
        {
            AModelBuilder.Entity<Parking>().HasData(
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
            );
        }
    }
}
