using ShopFComputerBackEnd.Notification.Domain.Enums;
using ShopFComputerBackEnd.Notification.Domain.ReadModels;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ShopFComputerBackEnd.Notification.Data
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<NotificationReadModel> Notifications { get; set; }
        public virtual DbSet<NotificationTemplateReadModel> Templates { get; set; }
        public virtual DbSet<HistoryReadModel> Histories { get; set; }
        public virtual DbSet<DeviceReadModel> Devices { get; set; }
        public ApplicationDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
#if DEBUG
            optionsBuilder.LogTo(Console.WriteLine);
#endif
        }

        static ApplicationDbContext()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<NotificationType>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<NotificationStatus>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<NotificationReadModel>().HasMany(entity => entity.Templates).WithOne(entity => entity.Notification).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<NotificationReadModel>().Property(entity => entity.Variables).HasColumnType("jsonb");
            modelBuilder.Entity<NotificationReadModel>().HasIndex(entity => entity.Id).IncludeProperties(entity => new { entity.Context, entity.Name, entity.Type, entity.Variables });
            modelBuilder.Entity<NotificationReadModel>().HasIndex(entity => entity.Context).IncludeProperties(entity => new { entity.Id, entity.Name, entity.Type, entity.Variables });
            modelBuilder.Entity<NotificationReadModel>().HasIndex(entity => new { entity.Context, entity.Name }).IncludeProperties(entity => new { entity.Id, entity.Type, entity.Variables });
            modelBuilder.Entity<NotificationReadModel>().HasIndex(entity => entity.Id).IncludeProperties(entity => new { entity.Context, entity.Name, entity.Type}).IsUnique();
            modelBuilder.HasPostgresEnum<NotificationType>();

            modelBuilder.Entity<NotificationTemplateReadModel>().Property(entity => entity.Attachments).HasColumnType("jsonb");
            modelBuilder.Entity<NotificationTemplateReadModel>().HasMany(entity => entity.Histories).WithOne(entity => entity.Template).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<NotificationTemplateReadModel>().HasIndex(entity => entity.Id).IncludeProperties(entity => new { entity.LanguageCode, entity.Subject, entity.Content });
            modelBuilder.Entity<NotificationTemplateReadModel>().HasIndex(entity => entity.LanguageCode).IncludeProperties(entity => new { entity.Id, entity.Subject, entity.Content });

            modelBuilder.Entity<HistoryReadModel>().Property(entity => entity.RawData).HasColumnType("jsonb");

            modelBuilder.HasPostgresEnum<NotificationStatus>();

            modelBuilder.Entity<DeviceReadModel>().HasKey(entity => new { entity.UserId, entity.Devicetoken , entity.ProfileId });
            modelBuilder.Entity<DeviceReadModel>().HasIndex(entity => entity.UserId).IncludeProperties(entity => new { entity.Devicetoken , entity.ProfileId });
            modelBuilder.Entity<DeviceReadModel>().HasIndex(entity => entity.ProfileId).IncludeProperties(entity => new { entity.Devicetoken , entity.UserId});
        }
    }
}
