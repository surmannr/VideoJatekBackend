﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VideoJatekBackend.Dal;

#nullable disable

namespace VideoJatekBackend.Migrations
{
    [DbContext(typeof(VideogameDbContext))]
    partial class VideogameDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("VideoJatekBackend.Models.Publisher", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Cim");

                    b.Property<DateTime>("FoundationDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("Alapitas_datuma");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Nev");

                    b.HasKey("Id");

                    b.ToTable("Kiado");
                });

            modelBuilder.Entity("VideoJatekBackend.Models.Videogame", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Kategoria");

                    b.Property<string>("Developer")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Fejleszto");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Nev");

                    b.Property<int>("PublisherId")
                        .HasColumnType("int")
                        .HasColumnName("KiadoId");

                    b.HasKey("Id");

                    b.HasIndex("PublisherId");

                    b.ToTable("Videojatek");
                });

            modelBuilder.Entity("VideoJatekBackend.Models.Videogame", b =>
                {
                    b.HasOne("VideoJatekBackend.Models.Publisher", "Publisher")
                        .WithMany("Videogames")
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Publisher");
                });

            modelBuilder.Entity("VideoJatekBackend.Models.Publisher", b =>
                {
                    b.Navigation("Videogames");
                });
#pragma warning restore 612, 618
        }
    }
}