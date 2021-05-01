using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CqrsDemo.Infrastructure.Domain.Entities;

namespace CqrsDemo.Infrastructure.Database.Mappings
{
    public class ParkingPlacesConfiguration : IEntityTypeConfiguration<ParkingPlace>
    {
        public void Configure(EntityTypeBuilder<ParkingPlace> AEntityBuilder)
        {
            AEntityBuilder
                .HasKey(AParkingPlace => new 
                { 
                    AParkingPlace.ParkingName, 
                    AParkingPlace.Number 
                });
            
            AEntityBuilder
                .HasOne(AParkingPlace => AParkingPlace.ParkingNameNavigation)
                .WithMany(AParking => AParking.ParkingPlaces)
                .HasForeignKey(AParkingPlace => AParkingPlace.ParkingName);
        }
    }
}
