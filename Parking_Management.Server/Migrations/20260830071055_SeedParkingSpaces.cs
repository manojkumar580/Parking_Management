using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Parking_Management.Server.Migrations
{
    /// <inheritdoc />
    public partial class SeedParkingSpaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ParkingSpaces",
                columns: new[] { "Id", "IsActive", "SpaceNumber", "SpaceType" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111001"), true, "2W-001", 1 },
                    { new Guid("11111111-1111-1111-1111-111111111002"), true, "2W-002", 1 },
                    { new Guid("11111111-1111-1111-1111-111111111003"), true, "2W-003", 1 },
                    { new Guid("22222222-2222-2222-2222-222222222001"), true, "4W-001", 2 },
                    { new Guid("22222222-2222-2222-2222-222222222002"), true, "4W-002", 2 },
                    { new Guid("22222222-2222-2222-2222-222222222003"), true, "4W-003", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ParkingSpaces",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111001"));

            migrationBuilder.DeleteData(
                table: "ParkingSpaces",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111002"));

            migrationBuilder.DeleteData(
                table: "ParkingSpaces",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111003"));

            migrationBuilder.DeleteData(
                table: "ParkingSpaces",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222001"));

            migrationBuilder.DeleteData(
                table: "ParkingSpaces",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222002"));

            migrationBuilder.DeleteData(
                table: "ParkingSpaces",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222003"));
        }
    }
}
