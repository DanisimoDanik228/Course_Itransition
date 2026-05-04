using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository.PostgresDbContext
{
    public class AppDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
           
        }

        public DbSet<Inventory> Inventory => Set<Inventory>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<ItemValue> ItemValue => Set<ItemValue>();
        public DbSet<InventoryType> InventoryType => Set<InventoryType>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Inventory>()
                .HasMany(i => i.Items)
                .WithOne(i => i.Inventory)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Inventory>()
                .HasMany(i => i.InventoryType)
                .WithOne(it => it.Inventory)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Item>()
                .HasMany(i => i.ItemValue)
                .WithOne(iv => iv.Item)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
