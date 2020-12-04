using Microsoft.EntityFrameworkCore;

namespace CqrsDemo.Database
{

    public class MainDbContext : DbContext
    {

        public MainDbContext(DbContextOptions<MainDbContext> AOptions) : base(AOptions)
        {
        }



    }

}