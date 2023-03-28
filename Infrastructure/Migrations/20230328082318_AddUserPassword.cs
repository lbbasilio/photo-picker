using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhotoPicker.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoData",
                table: "Photos");

            migrationBuilder.AddColumn<string>(
                name: "HashedPassword",
                table: "Users",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExtractedDate",
                table: "Photos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MediaType",
                table: "Photos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "S3Url",
                table: "Photos",
                type: "longtext",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HashedPassword",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ExtractedDate",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "S3Url",
                table: "Photos");

            migrationBuilder.AddColumn<byte[]>(
                name: "PhotoData",
                table: "Photos",
                type: "longblob",
                maxLength: 2097152,
                nullable: true);
        }
    }
}
