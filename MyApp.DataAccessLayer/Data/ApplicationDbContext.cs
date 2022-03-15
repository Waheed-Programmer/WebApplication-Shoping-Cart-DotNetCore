using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyApp.Models;


namespace MyApp.DataAccessLayer
{
    public class ApplicationDbContext:IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option):base(option)
        {

        }
        public DbSet<Category> Categories { get; set; } 
        public DbSet<Product> Products { get; set; } 
        public DbSet<Cart> Carts { get; set; } 
        public DbSet<ApplicationUser> ApplicationUsers { get; set; } 
        public DbSet<OrderHeader> orderHeaders { get; set; } 
        public DbSet<OrderDetail> orderDetails { get; set; } 
    }
}
