﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

using REST_uService.Models;

namespace REST_uService.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
            
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
