﻿// <auto-generated />
using System;
using Final_Project_Farrukh_Rakhmanov.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Final_Project_Farrukh_Rakhmanov.Migrations
{
    [DbContext(typeof(UserAccountContext))]
    [Migration("20250111030755_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Final_Project_Farrukh_Rakhmanov.Models.Book", b =>
                {
                    b.Property<int>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookId"));

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("AvailableQuantity")
                        .HasColumnType("int");

                    b.Property<string>("GenreId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("BookId");

                    b.HasIndex("GenreId");

                    b.ToTable("Books");

                    b.HasData(
                        new
                        {
                            BookId = 1,
                            Author = "Fitzgerald F. Scott",
                            AvailableQuantity = 4,
                            GenreId = "1",
                            IsAvailable = true,
                            Quantity = 5,
                            Title = "The Great Gatsby",
                            Year = 1925
                        },
                        new
                        {
                            BookId = 2,
                            Author = "Yuval Noah Harari",
                            AvailableQuantity = 5,
                            GenreId = "2",
                            IsAvailable = true,
                            Quantity = 5,
                            Title = "Sapiens",
                            Year = 2011
                        },
                        new
                        {
                            BookId = 3,
                            Author = "Frank Herbert",
                            AvailableQuantity = 5,
                            GenreId = "1",
                            IsAvailable = true,
                            Quantity = 5,
                            Title = "Dune",
                            Year = 1965
                        },
                        new
                        {
                            BookId = 4,
                            Author = "J.R.R. Tolkien",
                            AvailableQuantity = 5,
                            GenreId = "3",
                            IsAvailable = true,
                            Quantity = 5,
                            Title = "The Hobbit",
                            Year = 1937
                        },
                        new
                        {
                            BookId = 5,
                            Author = "J.D. Salinger",
                            AvailableQuantity = 5,
                            GenreId = "1",
                            IsAvailable = true,
                            Quantity = 5,
                            Title = "The Catcher in the Rye",
                            Year = 1951
                        });
                });

            modelBuilder.Entity("Final_Project_Farrukh_Rakhmanov.Models.Genre", b =>
                {
                    b.Property<string>("GenreId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("GenreId");

                    b.ToTable("Genres");

                    b.HasData(
                        new
                        {
                            GenreId = "1",
                            Name = "Fiction"
                        },
                        new
                        {
                            GenreId = "2",
                            Name = "Non-Fiction"
                        },
                        new
                        {
                            GenreId = "3",
                            Name = "Fantasy"
                        },
                        new
                        {
                            GenreId = "4",
                            Name = "Mystery"
                        },
                        new
                        {
                            GenreId = "5",
                            Name = "Science Fiction"
                        });
                });

            modelBuilder.Entity("Final_Project_Farrukh_Rakhmanov.Models.MemberBook", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.HasKey("Id", "BookId");

                    b.HasIndex("BookId");

                    b.ToTable("MemberBook");

                    b.HasData(
                        new
                        {
                            Id = 2,
                            BookId = 1
                        },
                        new
                        {
                            Id = 2,
                            BookId = 2
                        },
                        new
                        {
                            Id = 2,
                            BookId = 3
                        });
                });

            modelBuilder.Entity("Final_Project_Farrukh_Rakhmanov.Models.ProfilePicture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ProfilePictures");
                });

            modelBuilder.Entity("Final_Project_Farrukh_Rakhmanov.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("ConfirmPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProfilePictureId")
                        .HasColumnType("int");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProfilePictureId");

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("UserType").HasValue("User");

                    b.UseTphMappingStrategy();

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Age = 28,
                            ConfirmPassword = "Admin@123",
                            DateOfBirth = new DateTime(1996, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "John",
                            Password = "Admin@123",
                            Role = "Admin",
                            Username = "admin"
                        });
                });

            modelBuilder.Entity("Final_Project_Farrukh_Rakhmanov.Models.Member", b =>
                {
                    b.HasBaseType("Final_Project_Farrukh_Rakhmanov.Models.User");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("Member");

                    b.HasData(
                        new
                        {
                            Id = 2,
                            Age = 33,
                            ConfirmPassword = "Member@123",
                            DateOfBirth = new DateTime(1991, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Alex",
                            Password = "Member@123",
                            Role = "Member",
                            Username = "member",
                            Email = "john@gmail.com",
                            PhoneNumber = "123-456-7890"
                        });
                });

            modelBuilder.Entity("Final_Project_Farrukh_Rakhmanov.Models.Book", b =>
                {
                    b.HasOne("Final_Project_Farrukh_Rakhmanov.Models.Genre", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("Final_Project_Farrukh_Rakhmanov.Models.MemberBook", b =>
                {
                    b.HasOne("Final_Project_Farrukh_Rakhmanov.Models.Book", "Book")
                        .WithMany("MemberBooks")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Final_Project_Farrukh_Rakhmanov.Models.Member", "Member")
                        .WithMany("MemberBooks")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("Final_Project_Farrukh_Rakhmanov.Models.User", b =>
                {
                    b.HasOne("Final_Project_Farrukh_Rakhmanov.Models.ProfilePicture", "ProfilePicture")
                        .WithMany()
                        .HasForeignKey("ProfilePictureId");

                    b.Navigation("ProfilePicture");
                });

            modelBuilder.Entity("Final_Project_Farrukh_Rakhmanov.Models.Book", b =>
                {
                    b.Navigation("MemberBooks");
                });

            modelBuilder.Entity("Final_Project_Farrukh_Rakhmanov.Models.Member", b =>
                {
                    b.Navigation("MemberBooks");
                });
#pragma warning restore 612, 618
        }
    }
}
