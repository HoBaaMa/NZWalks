using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZWalks_API.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDataForDifficultiesAndRegions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("4a8fe1b6-a65c-4502-8e7d-8d8160dfcd2e"), "Medium" },
                    { new Guid("8daf33fd-ae63-4b9e-9dda-607430a8921d"), "Hard" },
                    { new Guid("e2975a33-fdb3-4636-b662-0481bc8cc67d"), "Easy" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { new Guid("14ceba71-4b51-4777-9b17-46602cf66153"), "BOP", null, "Bay Of Plenty" },
                    { new Guid("57da9f56-af31-4d60-b753-e90c01374d99"), "AKL", "https://www.pexels.com/photo/luxury-apartment-view-over-auckland-skyline-33634443/", "Auckland" },
                    { new Guid("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"), "NTL", null, "Northland" },
                    { new Guid("906cb139-415a-4bbb-a174-1a1faf9fb1f6"), "NSN", "https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1", "Nelson" },
                    { new Guid("cfa06ed2-bf65-4b65-93ed-c9d286ddb0de"), "WGN", "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1", "Wellington" },
                    { new Guid("f077a22e-4248-4bf6-b564-c7cf4e250263"), "STL", null, "Southland" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("4a8fe1b6-a65c-4502-8e7d-8d8160dfcd2e"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("8daf33fd-ae63-4b9e-9dda-607430a8921d"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("e2975a33-fdb3-4636-b662-0481bc8cc67d"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("14ceba71-4b51-4777-9b17-46602cf66153"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("57da9f56-af31-4d60-b753-e90c01374d99"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("906cb139-415a-4bbb-a174-1a1faf9fb1f6"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("cfa06ed2-bf65-4b65-93ed-c9d286ddb0de"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("f077a22e-4248-4bf6-b564-c7cf4e250263"));
        }
    }
}
