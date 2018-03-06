using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace store_scrapper_2.DAL.Db.Migrations
{
    public partial class AddUpdatedTimeUtcToStores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTimeUtc",
                table: "Stores",
                type: "timestamp",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateTimeUtc",
                table: "Stores");
        }
    }
}
