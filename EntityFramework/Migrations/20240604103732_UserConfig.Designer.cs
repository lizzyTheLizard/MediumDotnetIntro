﻿// <auto-generated />
using System;
using EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EntityFramework.Migrations
{
    [DbContext(typeof(ExampleDbContext))]
    [Migration("20240604103732_UserConfig")]
    partial class UserConfig
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("EntityFramework.Example", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .HasColumnType("varchar256")
                        .HasColumnName("Note2");

                    b.Property<int>("Value")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Examples");
                });

            modelBuilder.Entity("EntityFramework.SubExample", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ExampleId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Value")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ExampleId");

                    b.ToTable("SubExample");
                });

            modelBuilder.Entity("EntityFramework.UserConfiguration", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ExampleId")
                        .HasColumnType("TEXT");

                    b.HasKey("Name");

                    b.HasIndex("ExampleId");

                    b.ToTable("UserConfiguration");
                });

            modelBuilder.Entity("EntityFramework.SubExample", b =>
                {
                    b.HasOne("EntityFramework.Example", null)
                        .WithMany("SubExamples")
                        .HasForeignKey("ExampleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EntityFramework.UserConfiguration", b =>
                {
                    b.HasOne("EntityFramework.Example", "Example")
                        .WithMany()
                        .HasForeignKey("ExampleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Example");
                });

            modelBuilder.Entity("EntityFramework.Example", b =>
                {
                    b.Navigation("SubExamples");
                });
#pragma warning restore 612, 618
        }
    }
}
