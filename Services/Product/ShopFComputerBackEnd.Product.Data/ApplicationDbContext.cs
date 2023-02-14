using Microsoft.EntityFrameworkCore;
using ShopFComputerBackEnd.Product.Domain.ReadModels.Products;
using System.Diagnostics.CodeAnalysis;
using ShopFComputerBackEnd.Product.Domain.ReadModels.Options;
using System;

namespace ShopFComputerBackEnd.Product.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ProductVariantReadModel> ProductVariants { get; set; }
        public DbSet<OptionValueReadModel> OptionValues { get; set; }
        public DbSet<OptionReadModel> Options { get; set; }
        public DbSet<ProductReadModel> Products { get; set; }

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
            #region ProductModel

            modelBuilder.Entity<ProductReadModel>().HasKey(entity => entity.Id);
            modelBuilder.Entity<ProductReadModel>().HasIndex(entity => entity.Id).IncludeProperties(entity => new { entity.Name, entity.Description, entity.Category, entity.Brand,entity.IsDeleted, entity.CreatedBy, entity.CreatedTime, entity.ModifiedBy, entity.ModifiedTime, entity.DeletedBy, entity.DeletedTime }).IsUnique();
            modelBuilder.Entity<ProductReadModel>().HasIndex(entity => entity.Name).IncludeProperties(entity => new { entity.Id, entity.Description, entity.Category, entity.Brand, entity.IsDeleted, entity.CreatedBy, entity.CreatedTime, entity.ModifiedBy, entity.ModifiedTime, entity.DeletedBy, entity.DeletedTime }).IsUnique();

            #endregion

            #region ProductVariant

            modelBuilder.Entity<ProductVariantReadModel>().HasKey(entity => entity.Id);
            modelBuilder.Entity<ProductVariantReadModel>().Property(entity => entity.Images).HasColumnType("jsonb");

            modelBuilder.Entity<ProductVariantReadModel>().HasIndex(entity => entity.Id).IncludeProperties(entity => new { entity.SkuId, entity.ProductId, entity.ImportPrice, entity.Price, entity.Quantity, entity.IsDeleted }).IsUnique();
            modelBuilder.Entity<ProductVariantReadModel>().HasIndex(entity => entity.SkuId).IncludeProperties(entity => new { entity.Id, entity.ProductId, entity.ImportPrice, entity.Price, entity.Quantity, entity.IsDeleted }).IsUnique();
            modelBuilder.Entity<ProductVariantReadModel>().HasIndex(entity => entity.Price).IncludeProperties(entity => new { entity.Id, entity.ProductId, entity.ImportPrice, entity.SkuId, entity.Quantity, entity.IsDeleted });

            modelBuilder.Entity<ProductVariantReadModel>().HasOne(entity => entity.Product).WithMany(entity => entity.ProductVariants).HasForeignKey(entity => entity.ProductId);

            #endregion

            #region Option

            modelBuilder.Entity<OptionReadModel>().HasKey(entity => entity.Id);
            modelBuilder.Entity<OptionReadModel>().HasIndex(entity => entity.Id).IncludeProperties(entity => new { entity.Name, entity.IsDeleted }).IsUnique();
            modelBuilder.Entity<OptionReadModel>().HasIndex(entity => entity.Name).IncludeProperties(entity => new { entity.Id, entity.IsDeleted });

            modelBuilder.Entity<OptionReadModel>().HasOne(entity => entity.Product).WithMany(entity => entity.Options).HasForeignKey(entity => entity.ProductId);

            #endregion

            #region OptionValue

            modelBuilder.Entity<OptionValueReadModel>().HasKey(entity => entity.Id);
            modelBuilder.Entity<OptionValueReadModel>().Property(entity => entity.Value).HasDefaultValue("Updating");

            modelBuilder.Entity<OptionValueReadModel>().HasIndex(entity => entity.Id).IncludeProperties(entity => new { entity.OptionId, entity.ProductVariantId, entity.Value, entity.IsDeleted }).IsUnique();
            modelBuilder.Entity<OptionValueReadModel>().HasIndex(entity => new { entity.OptionId, entity.ProductVariantId }).IncludeProperties(entity => new { entity.Id, entity.Value, entity.IsDeleted }).IsUnique();
            modelBuilder.Entity<OptionValueReadModel>().HasIndex(entity => entity.Value).IncludeProperties(entity => new { entity.Id, entity.OptionId , entity.ProductVariantId, entity.IsDeleted });

            modelBuilder.Entity<OptionValueReadModel>().HasOne(entity => entity.ProductVariant).WithMany(entity => entity.OptionValues).HasForeignKey(entity => entity.ProductVariantId);
            modelBuilder.Entity<OptionValueReadModel>().HasOne(entity => entity.Option).WithMany(entity => entity.OptionValues).HasForeignKey(entity => entity.OptionId);

            #endregion
        }
    }
}
