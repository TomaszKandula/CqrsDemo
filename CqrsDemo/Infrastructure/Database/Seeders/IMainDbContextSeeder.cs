using Microsoft.EntityFrameworkCore;

namespace CqrsDemo.Infrastructure.Database.Seeders
{
    public interface IMainDbContextSeeder
    {
        void Seed(ModelBuilder AModelBuilder);
    }
}
