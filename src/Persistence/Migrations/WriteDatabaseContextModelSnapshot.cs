﻿// <auto-generated />
using System;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(WriteDatabaseContext))]
    partial class WriteDatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entity.AdminEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("Domain.Entity.BillingPartyEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Balance")
                        .HasColumnType("double precision");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<string>("VatNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("VatNumber")
                        .IsUnique();

                    b.ToTable("BillingParties");
                });

            modelBuilder.Entity("Domain.Entity.IncomeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<Guid>("BillingPartyId")
                        .HasColumnType("uuid");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Remarks")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("BillingPartyId");

                    b.ToTable("Incomes");
                });

            modelBuilder.Entity("Domain.Entity.ItemEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("CurrentEstimatedStockValuePerKilo")
                        .HasColumnType("double precision");

                    b.Property<double>("CurrentStockAmount")
                        .HasColumnType("double precision");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Domain.Entity.PurchaseEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BillingPartyId")
                        .HasColumnType("uuid");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<int?>("InvoiceNumber")
                        .HasColumnType("integer");

                    b.Property<double?>("PaidAmount")
                        .HasColumnType("double precision");

                    b.Property<string>("Remarks")
                        .HasColumnType("text");

                    b.Property<double?>("TransportFee")
                        .HasColumnType("double precision");

                    b.Property<double?>("VatAmount")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("BillingPartyId");

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("Domain.Entity.IncomeEntity", b =>
                {
                    b.HasOne("Domain.Entity.BillingPartyEntity", "BillingParty")
                        .WithMany()
                        .HasForeignKey("BillingPartyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BillingParty");
                });

            modelBuilder.Entity("Domain.Entity.ItemEntity", b =>
                {
                    b.OwnsMany("Domain.Entity.Stock", "StockHistory", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uuid");

                            b1.Property<DateOnly>("Date")
                                .HasColumnType("date");

                            b1.Property<string>("EntryCategory")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<double>("ExpectedValuePerKilo")
                                .HasColumnType("double precision");

                            b1.Property<Guid>("ItemEntityId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Remarks")
                                .HasColumnType("text");

                            b1.Property<double>("Weight")
                                .HasColumnType("double precision");

                            b1.HasKey("Id");

                            b1.HasIndex("ItemEntityId");

                            b1.ToTable("Stock");

                            b1.WithOwner()
                                .HasForeignKey("ItemEntityId");
                        });

                    b.Navigation("StockHistory");
                });

            modelBuilder.Entity("Domain.Entity.PurchaseEntity", b =>
                {
                    b.HasOne("Domain.Entity.BillingPartyEntity", "BillingParty")
                        .WithMany()
                        .HasForeignKey("BillingPartyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("Domain.Entity.PurchaseLineItem", "Purchases", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uuid");

                            b1.Property<Guid>("ItemEntityId")
                                .HasColumnType("uuid");

                            b1.Property<double>("Price")
                                .HasColumnType("double precision");

                            b1.Property<Guid>("PurchaseEntityId")
                                .HasColumnType("uuid");

                            b1.Property<double>("Quantity")
                                .HasColumnType("double precision");

                            b1.Property<double?>("Report")
                                .HasColumnType("double precision");

                            b1.HasKey("Id");

                            b1.HasIndex("ItemEntityId");

                            b1.HasIndex("PurchaseEntityId");

                            b1.ToTable("PurchaseLineItem");

                            b1.HasOne("Domain.Entity.ItemEntity", "ItemEntity")
                                .WithMany()
                                .HasForeignKey("ItemEntityId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.WithOwner()
                                .HasForeignKey("PurchaseEntityId");

                            b1.Navigation("ItemEntity");
                        });

                    b.Navigation("BillingParty");

                    b.Navigation("Purchases");
                });
#pragma warning restore 612, 618
        }
    }
}
