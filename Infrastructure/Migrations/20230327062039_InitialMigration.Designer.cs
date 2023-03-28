﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PhotoPicker.Infrastructure;

#nullable disable

namespace PhotoPicker.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230327062039_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("PhotoPicker.Entities.Photo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<int?>("AlogrithmConfidence")
                        .HasColumnType("int");

                    b.Property<byte[]>("PhotoData")
                        .HasColumnType("longblob");

                    b.Property<long?>("RecognizedUserId")
                        .HasColumnType("bigint");

                    b.Property<long?>("SelectedUserId")
                        .HasColumnType("bigint");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<DateTime>("UploadDate")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("UploaderId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("RecognizedUserId");

                    b.HasIndex("SelectedUserId");

                    b.HasIndex("UploaderId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("PhotoPicker.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("LastLogin")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PhotoPicker.Entities.Photo", b =>
                {
                    b.HasOne("PhotoPicker.Entities.User", "RecognizedUser")
                        .WithMany()
                        .HasForeignKey("RecognizedUserId");

                    b.HasOne("PhotoPicker.Entities.User", "SelectedUser")
                        .WithMany()
                        .HasForeignKey("SelectedUserId");

                    b.HasOne("PhotoPicker.Entities.User", "Uploader")
                        .WithMany()
                        .HasForeignKey("UploaderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RecognizedUser");

                    b.Navigation("SelectedUser");

                    b.Navigation("Uploader");
                });
#pragma warning restore 612, 618
        }
    }
}
