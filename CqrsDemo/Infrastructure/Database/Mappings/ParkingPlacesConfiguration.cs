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
                .HasKey(AEntity => new 
                { 
                    AEntity.ParkingName, 
                    AEntity.Number 
                });
            AEntityBuilder
                .HasOne(d => d.ParkingNameNavigation)
                .WithMany(p => p.ParkingPlaces)
                .HasForeignKey(d => d.ParkingName);
        }
    }
}
