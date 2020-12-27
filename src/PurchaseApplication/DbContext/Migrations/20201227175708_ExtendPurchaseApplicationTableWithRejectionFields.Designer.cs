﻿// <auto-generated />
using System;
using CanaryDeliveries.PurchaseApplication.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CanaryDeliveries.PurchaseApplication.DbContext.Migrations
{
    [DbContext(typeof(PurchaseApplicationDbContext))]
    [Migration("20201227175708_ExtendPurchaseApplicationTableWithRejectionFields")]
    partial class ExtendPurchaseApplicationTableWithRejectionFields
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("CanaryDeliveries.PurchaseApplication.DbContext.Client", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("character varying(16)")
                        .HasMaxLength(16);

                    b.HasKey("Id");

                    b.ToTable("PurchaseApplication_Clients");
                });

            modelBuilder.Entity("CanaryDeliveries.PurchaseApplication.DbContext.Product", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AdditionalInformation")
                        .HasColumnType("character varying(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("character varying(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("PromotionCode")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("PurchaseApplicationId")
                        .HasColumnType("text");

                    b.Property<int>("Units")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PurchaseApplicationId");

                    b.ToTable("PurchaseApplication_Products");
                });

            modelBuilder.Entity("CanaryDeliveries.PurchaseApplication.DbContext.PurchaseApplication", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AdditionalInformation")
                        .HasColumnType("character varying(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("RejectionDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("RejectionReason")
                        .HasColumnType("character varying(1000)")
                        .HasMaxLength(1000);

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("PurchaseApplication_PurchaseApplications");
                });

            modelBuilder.Entity("CanaryDeliveries.PurchaseApplication.DbContext.Product", b =>
                {
                    b.HasOne("CanaryDeliveries.PurchaseApplication.DbContext.PurchaseApplication", null)
                        .WithMany("Products")
                        .HasForeignKey("PurchaseApplicationId");
                });

            modelBuilder.Entity("CanaryDeliveries.PurchaseApplication.DbContext.PurchaseApplication", b =>
                {
                    b.HasOne("CanaryDeliveries.PurchaseApplication.DbContext.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
