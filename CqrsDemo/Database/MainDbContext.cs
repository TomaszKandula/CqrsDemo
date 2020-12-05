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

        public DbSet<Parking> Parking { get; set; }
        public DbSet<ParkingPlace> ParkingPlaces { get; set; }
        public DbSet<Command> CommandStore { get; set; }

        protected override void OnModelCreating(ModelBuilder AModelBuilder)
        {
            AModelBuilder.Entity<Parking>()
                .HasKey(Entity => Entity.Name);

            AModelBuilder.Entity<Parking>()
                .HasMany(Entity => Entity.Places)
                .WithOne(Entity => Entity.Parking)
                .HasForeignKey(Entity => Entity.ParkingName)
                .IsRequired();

            AModelBuilder.Entity<ParkingPlace>()
                .HasKey(Entity => new 
                { 
                    Entity.ParkingName, 
                    Entity.Number 
                });

            AModelBuilder.Entity<Command>()
                .HasKey(Command => Command.Id);
        }

    }

}