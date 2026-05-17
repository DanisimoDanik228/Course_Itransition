using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository.PostgresDbContext
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
           
        }

        public DbSet<Inventory> Inventory => Set<Inventory>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<ItemValue> ItemValue => Set<ItemValue>();
        public DbSet<InventoryType> InventoryType => Set<InventoryType>();
        public DbSet<EditorInventory> EditorInventory => Set<EditorInventory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<EditorInventory>()
                .HasIndex(e => new { e.EditorId, e.InventoryId })
                .IsUnique();

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

            modelBuilder.Entity<InventoryType>()
                .HasMany(i => i.ItemValue)
                .WithOne(iv => iv.InventoryType)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Creator)
                .WithMany(c => c.CreatedInventory)
                .HasForeignKey(i => i.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Inventory>()
                .HasMany(i => i.Editors)
                .WithOne(u => u.Inventory)
                .HasForeignKey(i => i.InventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppUser>()
                .HasMany(i => i.EditInventory)
                .WithOne(u => u.Editor)
                .HasForeignKey(i => i.EditorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
