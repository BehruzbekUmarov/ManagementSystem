using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InnerSystem.Identity.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("9104869e-8447-477d-b223-73caa2a09f21"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("db8af5a4-18ec-4c7a-afa0-e7eb68f201b2"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("44f42841-6295-48e3-85c0-9cfb516a26e0"), null, "Role", "User", "USER" },
                    { new Guid("4a0ca0d5-3778-454d-b73b-847d912cf349"), null, "Role", "Manager", "MANAGER" },
                    { new Guid("ecb21384-d2cd-40ee-ad39-043ea27d2882"), null, "Role", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("44f42841-6295-48e3-85c0-9cfb516a26e0"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4a0ca0d5-3778-454d-b73b-847d912cf349"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ecb21384-d2cd-40ee-ad39-043ea27d2882"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("9104869e-8447-477d-b223-73caa2a09f21"), null, "Role", "Admin", "ADMIN" },
                    { new Guid("db8af5a4-18ec-4c7a-afa0-e7eb68f201b2"), null, "Role", "User", "USER" }
                });
        }
    }
}
