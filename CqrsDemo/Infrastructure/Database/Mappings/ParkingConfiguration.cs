using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CqrsDemo.Infrastructure.Domain.Entities;

namespace CqrsDemo.Infrastructure.Database.Mappings
{
    public class ParkingConfiguration : IEntityTypeConfiguration<Parking>
    {
        public void Configure(EntityTypeBuilder<Parking> AEntityBuilder)
        {
            AEntityBuilder.HasKey(AParking => AParking.Name);
            AEntityBuilder.ToTable("Parking");
        }
    }
}
