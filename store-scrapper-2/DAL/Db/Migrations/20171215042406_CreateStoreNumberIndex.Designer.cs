﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using store_scrapper_2.DAL.Db;
using System;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace store_scrapper_2.DAL.Db.Migrations
{
    [DbContext(typeof(StoreDataContext))]
    [Migration("20171215042406_CreateStoreNumberIndex")]
    partial class CreateStoreNumberIndex
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("store_scrapper_2.DAL.Db.Store", b =>
                {
                    b.Property<int>("StoreId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address1");

                    b.Property<string>("Address2");

                    b.Property<string>("Address3");

                    b.Property<string>("CateringUrl");

                    b.Property<string>("City");

                    b.Property<string>("CountryCode");

                    b.Property<string>("CountryCode3");

                    b.Property<int>("CurrentUtcOffset");

                    b.Property<bool>("IsRestricted");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<string>("OrderingUrl");

                    b.Property<string>("PostalCode");

                    b.Property<string>("SatelliteNumber");

                    b.Property<string>("State");

                    b.Property<string>("StoreNumber");

                    b.Property<string>("TimeZoneId");

                    b.Property<DateTime?>("UpdateTimeUtc");

                    b.HasKey("StoreId");

                    b.HasIndex("StoreNumber", "SatelliteNumber");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("store_scrapper_2.DAL.Db.Zip", b =>
                {
                    b.Property<string>("ZipCode")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Latitude");

                    b.Property<decimal>("Longitude");

                    b.HasKey("ZipCode");

                    b.ToTable("Zips");
                });
#pragma warning restore 612, 618
        }
    }
}
