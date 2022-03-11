﻿using Microsoft.EntityFrameworkCore;
using MyApp.Models;


namespace MyApp.DataAccessLayer
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option):base(option)
        {

        }
        public DbSet<Category> Categories { get; set; } 
        public DbSet<Product> Products { get; set; } 
    }
}