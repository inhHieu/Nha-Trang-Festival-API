﻿// <auto-generated />
using System;
using Festival.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Festival.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230103030906_InitalCreate3")]
    partial class InitalCreate3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Festival.Models.Categories", b =>
                {
                    b.Property<int>("Category_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Category_ID"));

                    b.Property<string>("CategoryDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Category_ID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Festival.Models.Events", b =>
                {
                    b.Property<int>("Event_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Event_ID"));

                    b.Property<int>("CategoriesCategory_ID")
                        .HasColumnType("int");

                    b.Property<int>("Category_ID")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateStart")
                        .HasColumnType("datetime2");

                    b.Property<string>("EventDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TakePlace")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Event_ID");

                    b.HasIndex("CategoriesCategory_ID");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Festival.Models.Login", b =>
                {
                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserName");

                    b.ToTable("Login");
                });

            modelBuilder.Entity("Festival.Models.News", b =>
                {
                    b.Property<int>("News_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("News_ID"));

                    b.Property<int>("CategoriesCategory_ID")
                        .HasColumnType("int");

                    b.Property<int>("Category_ID")
                        .HasColumnType("int");

                    b.Property<string>("NewsContent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NewsTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PostedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Views")
                        .HasColumnType("int");

                    b.HasKey("News_ID");

                    b.HasIndex("CategoriesCategory_ID");

                    b.ToTable("News");
                });

            modelBuilder.Entity("Festival.Models.Subscribes", b =>
                {
                    b.Property<int>("Subscribe_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Subscribe_ID"));

                    b.Property<int>("Event_ID")
                        .HasColumnType("int");

                    b.Property<int>("EventsEvent_ID")
                        .HasColumnType("int");

                    b.Property<int>("User_ID")
                        .HasColumnType("int");

                    b.Property<int>("UsersUser_ID")
                        .HasColumnType("int");

                    b.HasKey("Subscribe_ID");

                    b.HasIndex("EventsEvent_ID");

                    b.HasIndex("UsersUser_ID");

                    b.ToTable("Subscribes");
                });

            modelBuilder.Entity("Festival.Models.Users", b =>
                {
                    b.Property<int>("User_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("User_ID"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Phone")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("User_ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Festival.Models.Events", b =>
                {
                    b.HasOne("Festival.Models.Categories", "Categories")
                        .WithMany("Event")
                        .HasForeignKey("CategoriesCategory_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categories");
                });

            modelBuilder.Entity("Festival.Models.News", b =>
                {
                    b.HasOne("Festival.Models.Categories", "Categories")
                        .WithMany("New")
                        .HasForeignKey("CategoriesCategory_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categories");
                });

            modelBuilder.Entity("Festival.Models.Subscribes", b =>
                {
                    b.HasOne("Festival.Models.Events", "Events")
                        .WithMany("Subscribe")
                        .HasForeignKey("EventsEvent_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Festival.Models.Users", "Users")
                        .WithMany("Subscribe")
                        .HasForeignKey("UsersUser_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Events");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Festival.Models.Categories", b =>
                {
                    b.Navigation("Event");

                    b.Navigation("New");
                });

            modelBuilder.Entity("Festival.Models.Events", b =>
                {
                    b.Navigation("Subscribe");
                });

            modelBuilder.Entity("Festival.Models.Users", b =>
                {
                    b.Navigation("Subscribe");
                });
#pragma warning restore 612, 618
        }
    }
}
