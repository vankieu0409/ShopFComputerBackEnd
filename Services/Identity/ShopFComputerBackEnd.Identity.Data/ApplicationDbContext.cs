using ShopFComputerBackEnd.Identity.Domain.ReadModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Diagnostics.CodeAnalysis;
using ShopFComputerBackEnd.Identity.Domain.Enums;

namespace ShopFComputerBackEnd.Identity.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUserReadModel, ApplicationRoleReadModel, Guid>
    {
        public virtual DbSet<FunctionReadModel> Functions { get; set; }
        public virtual DbSet<PermissionReadModel> Permissions { get; set; }
        public virtual DbSet<RefreshTokenReadModel> RefreshTokens { get; set; }

        public ApplicationDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }
        static ApplicationDbContext() => NpgsqlConnection.GlobalTypeMapper.MapEnum<PermissionType>();

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
            modelBuilder.Entity<FunctionReadModel>().HasIndex(entity => entity.Id).IncludeProperties(entity => new { entity.ServiceName, entity.FunctionName, entity.CreatedBy, entity.CreatedTime, entity.ModifiedBy, entity.ModifiedTime }).IsUnique();
            modelBuilder.Entity<PermissionReadModel>().HasKey(entity => new { entity.Type, entity.TypeId, entity.FunctionId });
            modelBuilder.HasPostgresEnum<PermissionType>();

            modelBuilder.Entity<RefreshTokenReadModel>().HasKey(entity => new { entity.Id });
            modelBuilder.Entity<RefreshTokenReadModel>().HasIndex(entity => entity.Id).IncludeProperties(entity => new { entity.UserId, entity.DeviceId, entity.RefreshToken, entity.ExpiredTime,entity.RevokedTime, entity.DeviceInfo });
            modelBuilder.Entity<RefreshTokenReadModel>().HasIndex(entity => entity.RefreshToken).IncludeProperties(entity => new { entity.Id, entity.UserId, entity.DeviceId, entity.ExpiredTime, entity.RevokedTime, entity.DeviceInfo });
            modelBuilder.Entity<RefreshTokenReadModel>().Property(entity => entity.DeviceInfo).HasColumnType("jsonb");
        }
    }
}
