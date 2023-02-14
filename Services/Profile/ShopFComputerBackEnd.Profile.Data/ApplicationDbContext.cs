using ShopFComputerBackEnd.Profile.Domain.Enums;
using ShopFComputerBackEnd.Profile.Domain.ReadModels;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ShopFComputerBackEnd.Profile.Data
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<ProfileReadModel> Profiles { get; set; }
        public ApplicationDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        static ApplicationDbContext()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<GendersType>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
#if DEBUG
            optionsBuilder.LogTo(Console.WriteLine);
#endif
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<ProfileReadModel>().HasKey(entity => entity.Id);
            modelBuilder.Entity<ProfileReadModel>().HasIndex(entity => entity.Id).IncludeProperties(entity => new { entity.Email, entity.PhoneNumber, entity.Address, entity.Avatar, entity.DisplayName, entity.Gender,entity.CreatedBy, entity.CreatedTime, entity.ModifiedBy, entity.ModifiedTime}).IsUnique();

            modelBuilder.Entity<ProfileReadModel>().Property(b => b.Avatar).HasColumnType("jsonb");
            modelBuilder.Entity<ProfileReadModel>().Property(b => b.Address).HasColumnType("jsonb");

            modelBuilder.HasPostgresEnum<GendersType>();
        }

    }
}
