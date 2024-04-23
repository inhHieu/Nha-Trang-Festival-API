using Microsoft.EntityFrameworkCore;
using Festival.Models;

namespace Festival.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base (options)
        {

        }
        public DbSet<Users> Users { get; set; }
        //public DbSet<Subscribed> Subscribes  { get; set; }
        public DbSet<Categories> Categories  { get; set; }
        public DbSet<Events> Events  { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Login> Login { get; set; }
        //public DbSet<Role> Role { get; set; }

    }
}
