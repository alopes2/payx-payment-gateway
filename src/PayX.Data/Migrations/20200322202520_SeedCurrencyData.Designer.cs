﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PayX.Data;

namespace PayX.Data.Migrations
{
    [DbContext(typeof(PayXDbContext))]
    [Migration("20200322202520_SeedCurrencyData")]
    partial class SeedCurrencyData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PayX.Core.Models.Currency", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Currencies");

                    b.HasData(
                        new
                        {
                            Id = new Guid("c48ab9b3-1f23-461a-a993-c6c040eb7d6b"),
                            Name = "EUR"
                        },
                        new
                        {
                            Id = new Guid("ab8ab9b3-1b23-46aa-aba3-c6c040eb7d6c"),
                            Name = "USD"
                        },
                        new
                        {
                            Id = new Guid("c48ab9c3-af23-461a-a103-c6c040eb7aba"),
                            Name = "BRL"
                        });
                });

            modelBuilder.Entity("PayX.Core.Models.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(5,2)");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(22)")
                        .HasMaxLength(22);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CurrencyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Cvv")
                        .HasColumnType("int")
                        .HasMaxLength(999);

                    b.Property<int>("ExpirationMonth")
                        .HasColumnType("int");

                    b.Property<int>("ExpirationYear")
                        .HasColumnType("int");

                    b.Property<bool>("IsSuccessful")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("PayX.Core.Models.Payment", b =>
                {
                    b.HasOne("PayX.Core.Models.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
