using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EraShop.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedSellerRoleToRolesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[] { "0574c1f8-3801-4810-8622-bcf466bc4df4", "08867cec-b9f8-44f4-b85c-425a1061ff09", true, false, "Seller", "SELLER" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAR2V+bcDJAlzUiuTRqKkLj/Uv4ibKCWikvvMF1g75/iOokLhV1l9SedoJOqspT0mA==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0574c1f8-3801-4810-8622-bcf466bc4df4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHWCK9fltC+ufLB3pWVTkNobLc2WWFCj3D1OJsdRppcwu7DRchtykHEPJHxnltfhhQ==");
        }
    }
}
