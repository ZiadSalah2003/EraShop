using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EraShop.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateListUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lists_Name",
                table: "Lists");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEK2Kc8lS82NgHqfJWwfOjFlcQZQQWzaHR02/KWFakPIXf34Eh6A9r8TLj7zoIbk5ug==");

            migrationBuilder.CreateIndex(
                name: "IX_Lists_Name_UserId",
                table: "Lists",
                columns: new[] { "Name", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lists_Name_UserId",
                table: "Lists");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKtEBhCf6fRdqaw3DvI47/Px6bWb5sOYM0kYRx0f+OXqCX0Ia6+FjWftMvRNp++iJg==");

            migrationBuilder.CreateIndex(
                name: "IX_Lists_Name",
                table: "Lists",
                column: "Name",
                unique: true);
        }
    }
}
