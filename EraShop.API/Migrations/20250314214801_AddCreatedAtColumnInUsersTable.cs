using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EraShop.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtColumnInUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 3, 14, 21, 47, 58, 663, DateTimeKind.Utc).AddTicks(1749), "AQAAAAIAAYagAAAAEFgNbwCMOer41vv1YSPKKCEODzdS2aT4TtzalZwIvRr0WmbTDm22cxf+ZcUDli5jkQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENcdbMfgNmhzNbm0dW60a1kXQnvT8zSZdsee5aBXouGf//26+YJY/Gn2VQg35doA3Q==");
        }
    }
}
