﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using newsscraper.DAL;

namespace newsscraper.Migrations
{
    [DbContext(typeof(ScraperContext))]
    [Migration("20180619075504_ArticleCreationDate")]
    partial class ArticleCreationDate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("newsscraper.DAL.Entities.Article", b =>
                {
                    b.Property<int>("ArticleID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Contents");

                    b.Property<DateTime?>("CreationDate");

                    b.Property<string>("Title");

                    b.Property<string>("Uri")
                        .IsRequired();

                    b.HasKey("ArticleID");

                    b.HasIndex("Uri")
                        .IsUnique();

                    b.ToTable("Articles");
                });
#pragma warning restore 612, 618
        }
    }
}
