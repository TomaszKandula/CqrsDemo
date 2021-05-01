using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CqrsDemo.Infrastructure.Domain.Entities;

namespace CqrsDemo.Infrastructure.Database.Mappings
{
    public class CommandStoreConfiguration : IEntityTypeConfiguration<CommandStore>
    {
        public void Configure(EntityTypeBuilder<CommandStore> AEntityBuilder)
            => AEntityBuilder.ToTable("CommandStore");
    }
}
