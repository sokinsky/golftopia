﻿// <auto-generated />
using System;
using GTA.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GTA.Server.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20231220204631_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GTA.Data.Models.Address", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("CityID")
                        .HasColumnType("int");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subpremise")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Zip")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("CityID");

                    b.ToTable("Addresses");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            CityID = 1,
                            Street = "2500 S Independence Blvd",
                            Zip = "23456"
                        });
                });

            modelBuilder.Entity("GTA.Data.Models.City", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StateID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("StateID");

                    b.ToTable("Cities");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Name = "Virginia Beach",
                            StateID = 1
                        });
                });

            modelBuilder.Entity("GTA.Data.Models.Email", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ID");

                    b.HasAlternateKey("Address");

                    b.ToTable("Emails");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Address = "sokinsky@gmail.com"
                        });
                });

            modelBuilder.Entity("GTA.Data.Models.Login", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Expired")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("Logins");
                });

            modelBuilder.Entity("GTA.Data.Models.Person", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("People");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            FirstName = "Steve",
                            LastName = "Okinsky"
                        },
                        new
                        {
                            ID = 2,
                            FirstName = "David",
                            LastName = "Okinsky"
                        },
                        new
                        {
                            ID = 3,
                            FirstName = "Nathan",
                            LastName = "Pannell"
                        },
                        new
                        {
                            ID = 4,
                            FirstName = "Bob",
                            LastName = "Souls"
                        },
                        new
                        {
                            ID = 5,
                            FirstName = "Ryan",
                            LastName = "Wheeler"
                        },
                        new
                        {
                            ID = 6,
                            FirstName = "Mathew",
                            LastName = "Hess"
                        },
                        new
                        {
                            ID = 7,
                            FirstName = "Brian",
                            LastName = "Corr"
                        },
                        new
                        {
                            ID = 8,
                            FirstName = "Danny",
                            LastName = "Martel"
                        });
                });

            modelBuilder.Entity("GTA.Data.Models.PersonEmail", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EmailID")
                        .HasColumnType("int");

                    b.Property<int>("PersonID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Verified")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasAlternateKey("PersonID", "EmailID");

                    b.HasIndex("EmailID");

                    b.ToTable("PeopleEmails");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Code = "0e5b2946-a7ab-417f-9798-4742bbe2667c",
                            EmailID = 1,
                            PersonID = 1
                        });
                });

            modelBuilder.Entity("GTA.Data.Models.PersonPhone", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PersonID")
                        .HasColumnType("int");

                    b.Property<int>("PhoneID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Verified")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasAlternateKey("PersonID", "PhoneID");

                    b.HasIndex("PhoneID");

                    b.ToTable("PeoplePhones");
                });

            modelBuilder.Entity("GTA.Data.Models.Phone", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ID");

                    b.HasAlternateKey("Number");

                    b.ToTable("Phones");
                });

            modelBuilder.Entity("GTA.Data.Models.State", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("States");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Code = "VA",
                            Name = "Virginia"
                        });
                });

            modelBuilder.Entity("GTA.Data.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ID");

                    b.HasAlternateKey("Username");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Password = "musk4rat!",
                            Username = "sokinsky"
                        });
                });

            modelBuilder.Entity("GTA.Data.Models.Venue", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int?>("AddressID")
                        .HasColumnType("int");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ParentID")
                        .HasColumnType("int");

                    b.Property<int?>("PhoneID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("AddressID");

                    b.HasIndex("ParentID");

                    b.HasIndex("PhoneID");

                    b.ToTable("Venue");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Venue");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("GTA.Data.Models.Course", b =>
                {
                    b.HasBaseType("GTA.Data.Models.Venue");

                    b.Property<string>("json_Tees")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Tees");

                    b.HasDiscriminator().HasValue("Course");
                });

            modelBuilder.Entity("GTA.Data.Models.Address", b =>
                {
                    b.HasOne("GTA.Data.Models.City", "City")
                        .WithMany()
                        .HasForeignKey("CityID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("GTA.Data.Models.City", b =>
                {
                    b.HasOne("GTA.Data.Models.State", "State")
                        .WithMany()
                        .HasForeignKey("StateID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("State");
                });

            modelBuilder.Entity("GTA.Data.Models.Login", b =>
                {
                    b.HasOne("GTA.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("GTA.Data.Models.PersonEmail", b =>
                {
                    b.HasOne("GTA.Data.Models.Email", "Email")
                        .WithMany()
                        .HasForeignKey("EmailID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GTA.Data.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Email");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("GTA.Data.Models.PersonPhone", b =>
                {
                    b.HasOne("GTA.Data.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GTA.Data.Models.Phone", "Phone")
                        .WithMany()
                        .HasForeignKey("PhoneID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");

                    b.Navigation("Phone");
                });

            modelBuilder.Entity("GTA.Data.Models.User", b =>
                {
                    b.HasOne("GTA.Data.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("GTA.Data.Models.Venue", b =>
                {
                    b.HasOne("GTA.Data.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressID");

                    b.HasOne("GTA.Data.Models.Venue", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentID");

                    b.HasOne("GTA.Data.Models.Phone", "Phone")
                        .WithMany()
                        .HasForeignKey("PhoneID");

                    b.Navigation("Address");

                    b.Navigation("Parent");

                    b.Navigation("Phone");
                });
#pragma warning restore 612, 618
        }
    }
}