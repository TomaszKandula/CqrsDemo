using System.Reflection;
using Microsoft.EntityFrameworkCore;
using CqrsDemo.Infrastructure.Domain.Entities;
using CqrsDemo.Infrastructure.Database.Seeders;

namespace CqrsDemo.Infrastructure.Database
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
            base.OnModelCreating(AModelBuilder);
            ApplyConfiguration(AModelBuilder);

            new ParkingSeeder().Seed(AModelBuilder);
            new ParkingPlaceSeeder().Seed(AModelBuilder);
            new CommandStoreSeeder().Seed(AModelBuilder);
        }

        protected void ApplyConfiguration(ModelBuilder AModelBuilder)
        {
            AModelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
