using Microsoft.EntityFrameworkCore;
using WelcomeWeb.Models;

namespace WelcomeWeb.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option):base(option)
        {

        }
        public DbSet<Category> Categories { get; set; } 
    }
}
