using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelEase.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureRoomTypeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomAmenityRoomType_RoomAmenities_AmenitiesId",
                table: "RoomAmenityRoomType");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomAmenityRoomType_RoomTypes_RoomTypesId",
                table: "RoomAmenityRoomType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomAmenityRoomType",
                table: "RoomAmenityRoomType");

            migrationBuilder.RenameTable(
                name: "RoomAmenityRoomType",
                newName: "RoomTypeAmenities");

            migrationBuilder.RenameIndex(
                name: "IX_RoomAmenityRoomType_RoomTypesId",
                table: "RoomTypeAmenities",
                newName: "IX_RoomTypeAmenities_RoomTypesId");

            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerNight",
                table: "RoomTypes",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "RoomTypes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomTypeAmenities",
                table: "RoomTypeAmenities",
                columns: new[] { "AmenitiesId", "RoomTypesId" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_RoomType_PriceRange",
                table: "RoomTypes",
                sql: "[PricePerNight] >= 0");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomTypeAmenities_RoomAmenities_AmenitiesId",
                table: "RoomTypeAmenities",
                column: "AmenitiesId",
                principalTable: "RoomAmenities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomTypeAmenities_RoomTypes_RoomTypesId",
                table: "RoomTypeAmenities",
                column: "RoomTypesId",
                principalTable: "RoomTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomTypeAmenities_RoomAmenities_AmenitiesId",
                table: "RoomTypeAmenities");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomTypeAmenities_RoomTypes_RoomTypesId",
                table: "RoomTypeAmenities");

            migrationBuilder.DropCheckConstraint(
                name: "CK_RoomType_PriceRange",
                table: "RoomTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomTypeAmenities",
                table: "RoomTypeAmenities");

            migrationBuilder.RenameTable(
                name: "RoomTypeAmenities",
                newName: "RoomAmenityRoomType");

            migrationBuilder.RenameIndex(
                name: "IX_RoomTypeAmenities_RoomTypesId",
                table: "RoomAmenityRoomType",
                newName: "IX_RoomAmenityRoomType_RoomTypesId");

            migrationBuilder.AlterColumn<float>(
                name: "PricePerNight",
                table: "RoomTypes",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "Category",
                table: "RoomTypes",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomAmenityRoomType",
                table: "RoomAmenityRoomType",
                columns: new[] { "AmenitiesId", "RoomTypesId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RoomAmenityRoomType_RoomAmenities_AmenitiesId",
                table: "RoomAmenityRoomType",
                column: "AmenitiesId",
                principalTable: "RoomAmenities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomAmenityRoomType_RoomTypes_RoomTypesId",
                table: "RoomAmenityRoomType",
                column: "RoomTypesId",
                principalTable: "RoomTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
