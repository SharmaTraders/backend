﻿// <auto-generated />
using System;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(WriteDatabaseContext))]
    [Migration("20240523203722_ModifyEmployee")]
    partial class ModifyEmployee
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("Domain.Entity.EmployeeEntity", b =>
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

                    b.Property<int>("NormalDailyWorkingMinute")
                        .HasColumnType("integer");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("PhoneNumber")
                        .IsUnique();

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Domain.Entity.ExpenseCategoryEntity", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Name");

                    b.ToTable("ExpenseCategories");
                });

            modelBuilder.Entity("Domain.Entity.ExpenseEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<Guid?>("BillingPartyId")
                        .HasColumnType("uuid");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Remarks")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("BillingPartyId");

                    b.HasIndex("CategoryName");

                    b.ToTable("Expenses");
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

            modelBuilder.Entity("Domain.Entity.SaleEntity", b =>
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

                    b.Property<double?>("ReceivedAmount")
                        .HasColumnType("double precision");

                    b.Property<string>("Remarks")
                        .HasColumnType("text");

                    b.Property<double?>("TransportFee")
                        .HasColumnType("double precision");

                    b.Property<double?>("VatAmount")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("BillingPartyId");

                    b.ToTable("Sales");
                });

            modelBuilder.Entity("Domain.Entity.EmployeeEntity", b =>
                {
                    b.OwnsMany("Domain.Entity.EmployeeSalaryRecord", "SalaryRecords", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uuid");

                            b1.Property<Guid>("EmployeeEntityId")
                                .HasColumnType("uuid");

                            b1.Property<DateOnly>("FromDate")
                                .HasColumnType("date");

                            b1.Property<double>("OvertimeSalaryPerHr")
                                .HasColumnType("double precision");

                            b1.Property<double>("SalaryPerHr")
                                .HasColumnType("double precision");

                            b1.Property<DateOnly?>("ToDate")
                                .HasColumnType("date");

                            b1.HasKey("Id");

                            b1.HasIndex("EmployeeEntityId");

                            b1.ToTable("EmployeeSalaryRecord");

                            b1.WithOwner()
                                .HasForeignKey("EmployeeEntityId");
                        });

                    b.OwnsMany("Domain.Entity.EmployeeWorkShift", "WorkShifts", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uuid");

                            b1.Property<int>("BreakMinutes")
                                .HasColumnType("integer");

                            b1.Property<DateOnly>("Date")
                                .HasColumnType("date");

                            b1.Property<Guid>("EmployeeEntityId")
                                .HasColumnType("uuid");

                            b1.Property<TimeOnly>("EndTime")
                                .HasColumnType("time without time zone");

                            b1.Property<TimeOnly>("StartTime")
                                .HasColumnType("time without time zone");

                            b1.HasKey("Id");

                            b1.HasIndex("Date");

                            b1.HasIndex("EmployeeEntityId");

                            b1.ToTable("EmployeeWorkShift");

                            b1.WithOwner()
                                .HasForeignKey("EmployeeEntityId");
                        });

                    b.Navigation("SalaryRecords");

                    b.Navigation("WorkShifts");
                });

            modelBuilder.Entity("Domain.Entity.ExpenseEntity", b =>
                {
                    b.HasOne("Domain.Entity.BillingPartyEntity", "BillingParty")
                        .WithMany()
                        .HasForeignKey("BillingPartyId");

                    b.HasOne("Domain.Entity.ExpenseCategoryEntity", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BillingParty");

                    b.Navigation("Category");
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

            modelBuilder.Entity("Domain.Entity.SaleEntity", b =>
                {
                    b.HasOne("Domain.Entity.BillingPartyEntity", "BillingParty")
                        .WithMany()
                        .HasForeignKey("BillingPartyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("Domain.Entity.SaleLineItem", "Sales", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uuid");

                            b1.Property<Guid>("ItemEntityId")
                                .HasColumnType("uuid");

                            b1.Property<double>("Price")
                                .HasColumnType("double precision");

                            b1.Property<double>("Quantity")
                                .HasColumnType("double precision");

                            b1.Property<double?>("Report")
                                .HasColumnType("double precision");

                            b1.Property<Guid>("SaleEntityId")
                                .HasColumnType("uuid");

                            b1.HasKey("Id");

                            b1.HasIndex("ItemEntityId");

                            b1.HasIndex("SaleEntityId");

                            b1.ToTable("SaleLineItem");

                            b1.HasOne("Domain.Entity.ItemEntity", "ItemEntity")
                                .WithMany()
                                .HasForeignKey("ItemEntityId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.WithOwner()
                                .HasForeignKey("SaleEntityId");

                            b1.Navigation("ItemEntity");
                        });

                    b.Navigation("BillingParty");

                    b.Navigation("Sales");
                });
#pragma warning restore 612, 618
        }
    }
}
