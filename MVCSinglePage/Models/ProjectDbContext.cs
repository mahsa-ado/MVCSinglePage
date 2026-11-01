using MVCSinglePage.Models.DomainModels.PersonAggregates;
using MVCSinglePage.Models.DomainModels.ProductAggregates;
using Microsoft.EntityFrameworkCore;

namespace MVCSinglePage.Models
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options)
           : base(options)
        {
        }

        public DbSet<Person> Person { get; set; }
        public DbSet<Product> Product { get; set; }
    }
}
