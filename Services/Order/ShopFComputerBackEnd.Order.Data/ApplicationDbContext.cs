using Microsoft.EntityFrameworkCore;

using ShopFComputerBackEnd.Order.Domain.ReadModels;

using System;
using System.Diagnostics.CodeAnalysis;
using ShopFComputerBackEnd.Order.Domain.Entities;

namespace ShopFComputerBackEnd.Order.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<OrderReadModel> Orders { get; set; }
        public DbSet<OrderDetailEntity> OrderDetails { get; set; }
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
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderReadModel>().ToTable("Orders");
            modelBuilder.Entity<OrderReadModel>().HasKey(entity => entity.Id);
            modelBuilder.Entity<OrderReadModel>().HasIndex(entity => entity.Id).IncludeProperties(entity => new { entity.ProfileId, entity.IsDeleted, entity.CreatedBy, entity.CreatedTime, entity.ModifiedBy, entity.ModifiedTime, entity.DeletedBy, entity.DeletedTime }).IsUnique();

            modelBuilder.Entity<OrderDetailEntity>().ToTable("OrderDetail");
            modelBuilder.Entity<OrderDetailEntity>().HasKey(entity => entity.Id);
            modelBuilder.Entity<OrderDetailEntity>().HasIndex(entity => entity.Id).IncludeProperties(entity => new { entity.OrderId }).IsUnique();
            modelBuilder.Entity<OrderDetailEntity>().HasIndex(entity => entity.OrderId).IncludeProperties(entity => new { entity.Id}).IsUnique();
            modelBuilder.Entity<OrderDetailEntity>().HasOne<OrderReadModel>(entity => entity.Order).WithMany(entity => entity.OrderDetails).HasForeignKey(entity => entity.OrderId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
