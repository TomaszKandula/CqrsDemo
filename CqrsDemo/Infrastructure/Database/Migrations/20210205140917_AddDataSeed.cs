using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CqrsDemo.Infrastructure.Database.Migrations
{
    public partial class AddDataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CommandStore",
                columns: new[] { "Id", "CreatedAt", "Data", "Type", "UserId" },
                values: new object[,]
                {
                    { 1L, new DateTime(2020, 12, 4, 20, 28, 3, 0, DateTimeKind.Unspecified), "{\"ParkingName\":\"Poznan Plaza\",\"Capacity\":2}", "CreateParking", "30fb43bf-9689-4a16-b41f-75775d11a02f" },
                    { 2L, new DateTime(2020, 12, 4, 20, 28, 3, 0, DateTimeKind.Unspecified), "{\"ParkingName\":\"Parking-786359\",\"Capacity\":2}", "CreateParking", "30fb43bf-9689-4a16-b41f-75775d11a02f" }
                });

            migrationBuilder.InsertData(
                table: "Parking",
                columns: new[] { "Name", "IsOpened" },
                values: new object[,]
                {
                    { "Poznan Plaza", false },
                    { "Parking-786359", true }
                });

            migrationBuilder.InsertData(
                table: "ParkingPlaces",
                columns: new[] { "Number", "ParkingName", "IsFree", "UserId" },
                values: new object[,]
                {
                    { 1, "Poznan Plaza", true, null },
                    { 2, "Poznan Plaza", true, null },
                    { 3, "Parking-786359", true, null },
                    { 4, "Parking-786359", false, null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CommandStore",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "CommandStore",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "ParkingPlaces",
                keyColumns: new[] { "Number", "ParkingName" },
                keyValues: new object[] { 3, "Parking-786359" });

            migrationBuilder.DeleteData(
                table: "ParkingPlaces",
                keyColumns: new[] { "Number", "ParkingName" },
                keyValues: new object[] { 4, "Parking-786359" });

            migrationBuilder.DeleteData(
                table: "ParkingPlaces",
                keyColumns: new[] { "Number", "ParkingName" },
                keyValues: new object[] { 1, "Poznan Plaza" });

            migrationBuilder.DeleteData(
                table: "ParkingPlaces",
                keyColumns: new[] { "Number", "ParkingName" },
                keyValues: new object[] { 2, "Poznan Plaza" });

            migrationBuilder.DeleteData(
                table: "Parking",
                keyColumn: "Name",
                keyValue: "Parking-786359");

            migrationBuilder.DeleteData(
                table: "Parking",
                keyColumn: "Name",
                keyValue: "Poznan Plaza");
        }
    }
}
