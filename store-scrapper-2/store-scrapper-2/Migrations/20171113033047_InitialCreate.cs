using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace store_scrapper_2.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    StoreId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Address1 = table.Column<string>(type: "TEXT", nullable: true),
                    Address2 = table.Column<string>(type: "TEXT", nullable: true),
                    Address3 = table.Column<string>(type: "TEXT", nullable: true),
                    CateringUrl = table.Column<string>(type: "TEXT", nullable: true),
                    City = table.Column<string>(type: "TEXT", nullable: true),
                    CountryCode = table.Column<string>(type: "TEXT", nullable: true),
                    CountryCode3 = table.Column<string>(type: "TEXT", nullable: true),
                    CurrentUtcOffset = table.Column<int>(type: "INTEGER", nullable: false),
                    IsRestricted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    ListingNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    OrderingUrl = table.Column<string>(type: "TEXT", nullable: true),
                    PostalCode = table.Column<string>(type: "TEXT", nullable: true),
                    SatelliteNumber = table.Column<string>(type: "TEXT", nullable: true),
                    State = table.Column<string>(type: "TEXT", nullable: true),
                    StoreNumber = table.Column<string>(type: "TEXT", nullable: true),
                    TimeZoneId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.StoreId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stores");
        }
    }
}
