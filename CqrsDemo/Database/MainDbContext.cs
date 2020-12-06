using CqrsDemo.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace CqrsDemo.Database
{

    public class MainDbContext : DbContext
    {

        public MainDbContext(DbContextOptions<MainDbContext> AOptions) : base(AOptions)
        {
        }

        public MainDbContext() 
        { 
        }

        public virtual DbSet<Parking> Parking { get; set; }
        public virtual DbSet<ParkingPlace> ParkingPlaces { get; set; }
        public virtual DbSet<CommandStore> CommandStore { get; set; }

        protected override void OnModelCreating(ModelBuilder AModelBuilder)
        {

            AModelBuilder.Entity<CommandStore>(AEntity =>
            {
                AEntity.ToTable("CommandStore");
            });

            AModelBuilder.Entity<Parking>(AEntity =>
            {
                AEntity.HasKey(AEntity => AEntity.Name);
                AEntity.ToTable("Parking");
            });

            AModelBuilder.Entity<ParkingPlace>(AEntity =>
            {
                AEntity.HasKey(AEntity => new { AEntity.ParkingName, AEntity.Number });
                AEntity.HasOne(d => d.ParkingNameNavigation)
                    .WithMany(p => p.ParkingPlaces)
                    .HasForeignKey(d => d.ParkingName);
            });

        }

    }

}