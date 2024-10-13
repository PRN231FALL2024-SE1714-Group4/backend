﻿// <auto-generated />
using System;
using BOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BOs.Migrations
{
    [DbContext(typeof(PiggeryManagementContext))]
    [Migration("20241013055846_AddSiftOfUser2")]
    partial class AddSiftOfUser2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BOs.Animal", b =>
                {
                    b.Property<Guid>("AnimalID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<int>("Breed")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("AnimalID");

                    b.ToTable("Animals");
                });

            modelBuilder.Entity("BOs.Area", b =>
                {
                    b.Property<Guid>("AreaID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("AreaID");

                    b.ToTable("Areas");
                });

            modelBuilder.Entity("BOs.Cage", b =>
                {
                    b.Property<Guid>("CageID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AreaID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CageName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("CageID");

                    b.HasIndex("AreaID");

                    b.ToTable("Cages");
                });

            modelBuilder.Entity("BOs.History", b =>
                {
                    b.Property<Guid>("AnimalID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CageID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("HistoryID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("AnimalID", "CageID");

                    b.HasIndex("CageID");

                    b.ToTable("Histories");
                });

            modelBuilder.Entity("BOs.Report", b =>
                {
                    b.Property<Guid>("ReportID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HealthDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("WorkId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ReportID");

                    b.HasIndex("WorkId");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("BOs.Role", b =>
                {
                    b.Property<Guid>("RoleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("RoleID");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("BOs.User", b =>
                {
                    b.Property<Guid>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("UserID");

                    b.HasIndex("RoleID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BOs.Work", b =>
                {
                    b.Property<Guid>("WorkId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AreaID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AssigneeID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AssignerID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CageID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Mission")
                        .HasColumnType("int");

                    b.Property<int>("Shift")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("WorkId");

                    b.HasIndex("AreaID");

                    b.HasIndex("AssigneeID");

                    b.HasIndex("AssignerID");

                    b.HasIndex("CageID");

                    b.ToTable("Works");
                });

            modelBuilder.Entity("BOs.Cage", b =>
                {
                    b.HasOne("BOs.Area", "Area")
                        .WithMany("Cages")
                        .HasForeignKey("AreaID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Area");
                });

            modelBuilder.Entity("BOs.History", b =>
                {
                    b.HasOne("BOs.Animal", "Animal")
                        .WithMany("Histories")
                        .HasForeignKey("AnimalID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BOs.Cage", "Cage")
                        .WithMany("Histories")
                        .HasForeignKey("CageID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Animal");

                    b.Navigation("Cage");
                });

            modelBuilder.Entity("BOs.Report", b =>
                {
                    b.HasOne("BOs.Work", "Work")
                        .WithMany("Reports")
                        .HasForeignKey("WorkId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Work");
                });

            modelBuilder.Entity("BOs.User", b =>
                {
                    b.HasOne("BOs.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("BOs.Work", b =>
                {
                    b.HasOne("BOs.Area", null)
                        .WithMany("Works")
                        .HasForeignKey("AreaID");

                    b.HasOne("BOs.User", "Assignee")
                        .WithMany("AssignedWorks")
                        .HasForeignKey("AssigneeID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BOs.User", "Assigner")
                        .WithMany("AssignedByMe")
                        .HasForeignKey("AssignerID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BOs.Cage", "Cage")
                        .WithMany()
                        .HasForeignKey("CageID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Assignee");

                    b.Navigation("Assigner");

                    b.Navigation("Cage");
                });

            modelBuilder.Entity("BOs.Animal", b =>
                {
                    b.Navigation("Histories");
                });

            modelBuilder.Entity("BOs.Area", b =>
                {
                    b.Navigation("Cages");

                    b.Navigation("Works");
                });

            modelBuilder.Entity("BOs.Cage", b =>
                {
                    b.Navigation("Histories");
                });

            modelBuilder.Entity("BOs.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("BOs.User", b =>
                {
                    b.Navigation("AssignedByMe");

                    b.Navigation("AssignedWorks");
                });

            modelBuilder.Entity("BOs.Work", b =>
                {
                    b.Navigation("Reports");
                });
#pragma warning restore 612, 618
        }
    }
}