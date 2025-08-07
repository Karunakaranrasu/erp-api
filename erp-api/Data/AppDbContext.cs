using erp_api.Entities;
using Microsoft.EntityFrameworkCore;

namespace erp_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
