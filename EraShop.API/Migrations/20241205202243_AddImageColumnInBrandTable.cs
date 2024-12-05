using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EraShop.API.Migrations
{
    /// <inheritdoc />
    public partial class AddImageColumnInBrandTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAED2XBrDiOSE5wPYkQBXFi2yAxeFzBcGQwfMna00c5VMMNgBkauj0zTOKGDsVZJu07w==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Brands");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEPPtSOyFCOqGsC1BsqNPSCN213CkHrMlXYVsGm83tW96IcIFXh7jeNiQJl/Wj4L3A==");
        }
    }
}
