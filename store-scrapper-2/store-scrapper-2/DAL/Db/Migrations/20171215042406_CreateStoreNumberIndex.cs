using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace store_scrapper_2.DAL.Db.Migrations
{
    public partial class CreateStoreNumberIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Stores_StoreNumber_SatelliteNumber",
                table: "Stores",
                columns: new[] { "StoreNumber", "SatelliteNumber" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stores_StoreNumber_SatelliteNumber",
                table: "Stores");
        }
    }
}
