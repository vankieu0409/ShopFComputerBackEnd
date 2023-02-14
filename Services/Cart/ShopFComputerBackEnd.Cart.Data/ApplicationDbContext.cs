using Microsoft.EntityFrameworkCore;

using Npgsql;

using ShopFComputerBackEnd.Cart.Domain.ReadModels;

using System;
using System.Diagnostics.CodeAnalysis;

namespace ShopFComputerBackEnd.Cart.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<CartReadModel> CartItems { get; set; }
        public ApplicationDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.LogTo(Console.WriteLine);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartReadModel>().ToTable("CART");
            modelBuilder.Entity<CartReadModel>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.HasIndex(c => c.Id).IncludeProperties(c => new { c.CreatedBy, c.CreatedTime, c.DeletedBy, c.DeletedTime, c.IsDeleted, c.ModifiedBy, c.ModifiedTime, c.Items });
                entity.HasIndex(c => c.ProfileId).IncludeProperties(c => new { c.CreatedBy, c.CreatedTime, c.DeletedBy, c.DeletedTime, c.IsDeleted, c.ModifiedBy, c.ModifiedTime, c.Items });
                entity.Property(c => c.Items).HasColumnType("jsonb");
            });

        }
    }
}
