﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ShopFComputerBackEnd.Cart.Data;
using ShopFComputerBackEnd.Cart.Domain.ReadModels;

#nullable disable

namespace ShopFComputerBackEnd.Cart.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20221107201704_CreateCart")]
    partial class CreateCart
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ShopFComputerBackEnd.Cart.Domain.ReadModels.CartReadModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnOrder(0);

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("DeletedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<ICollection<CartItemValueObject>>("Items")
                        .HasColumnType("jsonb");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("ModifiedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ProfileId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    NpgsqlIndexBuilderExtensions.IncludeProperties(b.HasIndex("Id"), new[] { "CreatedBy", "CreatedTime", "DeletedBy", "DeletedTime", "IsDeleted", "ModifiedBy", "ModifiedTime", "Items" });

                    b.HasIndex("ProfileId");

                    NpgsqlIndexBuilderExtensions.IncludeProperties(b.HasIndex("ProfileId"), new[] { "CreatedBy", "CreatedTime", "DeletedBy", "DeletedTime", "IsDeleted", "ModifiedBy", "ModifiedTime", "Items" });

                    b.ToTable("CART", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
