using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PayX.Data.Migrations
{
    public partial class SeedCurrencyData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("c48ab9b3-1f23-461a-a993-c6c040eb7d6b"), "EUR" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("ab8ab9b3-1b23-46aa-aba3-c6c040eb7d6c"), "USD" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("c48ab9c3-af23-461a-a103-c6c040eb7aba"), "BRL" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("ab8ab9b3-1b23-46aa-aba3-c6c040eb7d6c"));

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("c48ab9b3-1f23-461a-a993-c6c040eb7d6b"));

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("c48ab9c3-af23-461a-a103-c6c040eb7aba"));
        }
    }
}
