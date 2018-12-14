using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace store_scrapper_2.DAL.Db.Migrations
{
    public partial class RemoveZipIdColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Zips",
                table: "Zips");

            migrationBuilder.DropColumn(
                name: "ZipId",
                table: "Zips");

            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "Zips",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Zips",
                table: "Zips",
                column: "ZipCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Zips",
                table: "Zips");

            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "Zips",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "ZipId",
                table: "Zips",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Zips",
                table: "Zips",
                column: "ZipId");
        }
    }
}
