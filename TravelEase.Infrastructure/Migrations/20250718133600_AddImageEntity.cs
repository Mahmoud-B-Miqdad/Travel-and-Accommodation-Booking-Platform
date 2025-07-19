using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TravelEase.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddImageEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Discounts",
                keyColumn: "Id",
                keyValue: new Guid("246cf7e5-8c03-4cc2-bb6a-7592a15c0005"));

            migrationBuilder.DeleteData(
                table: "Discounts",
                keyColumn: "Id",
                keyValue: new Guid("342e5362-4a61-4d11-a7e7-6b01b47e0001"));

            migrationBuilder.DeleteData(
                table: "Discounts",
                keyColumn: "Id",
                keyValue: new Guid("7851f29b-6d7a-4f7c-83ea-4f1b8c9c0004"));

            migrationBuilder.DeleteData(
                table: "Discounts",
                keyColumn: "Id",
                keyValue: new Guid("cb51a724-e4ca-46e9-9e3d-d59f3f770002"));

            migrationBuilder.DeleteData(
                table: "Discounts",
                keyColumn: "Id",
                keyValue: new Guid("cfe7a21e-344d-4ae3-8a62-7cc89b710003"));

            migrationBuilder.DeleteData(
                table: "RoomAmenities",
                keyColumn: "Id",
                keyValue: new Guid("a11d00f2-4fc6-45f1-bc7a-3de3cc548603"));

            migrationBuilder.DeleteData(
                table: "RoomAmenities",
                keyColumn: "Id",
                keyValue: new Guid("b48fa00c-4f6d-4c91-a973-4cb94c80ec01"));

            migrationBuilder.DeleteData(
                table: "RoomAmenities",
                keyColumn: "Id",
                keyValue: new Guid("de1c3e3d-85f1-4c4e-9ec8-34de25173a04"));

            migrationBuilder.DeleteData(
                table: "RoomAmenities",
                keyColumn: "Id",
                keyValue: new Guid("e7c8c5ae-4411-4d57-ae1e-0e43a6cc6f05"));

            migrationBuilder.DeleteData(
                table: "RoomAmenities",
                keyColumn: "Id",
                keyValue: new Guid("f79904be-34aa-41ec-a580-40989e3b3602"));

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: new Guid("0e9a34a0-cf9e-4a41-b27c-96c3b5100008"));

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: new Guid("2690e45c-015f-4c58-8a33-0db4f3120010"));

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: new Guid("de2d1f5a-6237-4e49-8b10-24de1e0e0006"));

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: new Guid("10cdbbe9-1e91-4dc5-94e5-cfb6fce5c607"));

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: new Guid("1d2cbcb0-6727-4d3e-8c90-1c7c7e48f482"));

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: new Guid("99d8eb70-2190-4238-9f00-22f6e5b5a505"));

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: new Guid("b03c67e0-3c3c-4a24-9fa0-9632d693ab01"));

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: new Guid("e47fcdf4-6355-4ea3-a33f-59ff56ad1f03"));

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: new Guid("0af3d54f-214f-4c33-8a9e-2389329e0001"));

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: new Guid("0fa4010a-8c8a-4d8d-a6e7-3927c40a0002"));

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: new Guid("1a734e0b-78c3-401e-b8b4-32d129110009"));

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: new Guid("a2b8e135-84ef-4c34-83b1-1f68240b0003"));

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: new Guid("b4fca5e3-2e10-4b85-9f98-0b16d50c0004"));

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: new Guid("cb0f3c22-7a44-44a7-9e43-7cc3e70d0005"));

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: new Guid("fbc8e6f6-0fca-4a34-8f9d-41b37a0f0007"));

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: new Guid("04a5ffeb-9134-4097-9b4e-d4783c194102"));

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: new Guid("69702fd3-fb46-4694-b5fc-3de9c8b5a103"));

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: new Guid("9d6d68f6-c6b0-41a9-b6f1-72d7d340b101"));

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: new Guid("b3dd1dce-e79f-4d00-b5e3-f2d4d0a4f104"));

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: new Guid("fbfc9c27-799b-4d7e-b12a-dbbdc914f105"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("b6c9cf0e-31a6-4a35-a932-05f1e58f4a01"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("d3be3a21-8eac-48fa-a5f0-6c9e3c53ee03"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("f5f7c2b4-70a1-4b99-b6f2-8e416ab2de02"));

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Format = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CountryCode", "CountryName", "Name", "PostOffice" },
                values: new object[,]
                {
                    { new Guid("b6c9cf0e-31a6-4a35-a932-05f1e58f4a01"), "PS", "Palestine", "Jerusalem", "JRS001" },
                    { new Guid("d3be3a21-8eac-48fa-a5f0-6c9e3c53ee03"), "EG", "Egypt", "Cairo", "CAI003" },
                    { new Guid("f5f7c2b4-70a1-4b99-b6f2-8e416ab2de02"), "JO", "Jordan", "Amman", "AMN002" }
                });

            migrationBuilder.InsertData(
                table: "RoomAmenities",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("a11d00f2-4fc6-45f1-bc7a-3de3cc548603"), "42-inch flat screen television with cable channels.", "Flat Screen TV" },
                    { new Guid("b48fa00c-4f6d-4c91-a973-4cb94c80ec01"), "High-speed wireless internet access available throughout the room.", "Free WiFi" },
                    { new Guid("de1c3e3d-85f1-4c4e-9ec8-34de25173a04"), "Daily cleaning service to keep your room tidy.", "Daily Housekeeping" },
                    { new Guid("e7c8c5ae-4411-4d57-ae1e-0e43a6cc6f05"), "Free parking space available for guests.", "Parking" },
                    { new Guid("f79904be-34aa-41ec-a580-40989e3b3602"), "Individual air conditioning unit for personalized comfort.", "Air Conditioning" }
                });

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "Id", "CityId", "Description", "FloorsNumber", "Name", "OwnerName", "PhoneNumber", "Rating", "StreetAddress" },
                values: new object[,]
                {
                    { new Guid("04a5ffeb-9134-4097-9b4e-d4783c194102"), new Guid("b6c9cf0e-31a6-4a35-a932-05f1e58f4a01"), "Cozy boutique hotel near major landmarks.", 5, "Jerusalem Boutique Inn", "Sarah Cohen", "+970212345679", 4.3f, "45 King David Blvd." },
                    { new Guid("69702fd3-fb46-4694-b5fc-3de9c8b5a103"), new Guid("f5f7c2b4-70a1-4b99-b6f2-8e416ab2de02"), "Elegant hotel with modern amenities.", 8, "Amman Royal Hotel", "Omar Al-Khatib", "+96265123456", 4.5f, "12 Rainbow Street" },
                    { new Guid("9d6d68f6-c6b0-41a9-b6f1-72d7d340b101"), new Guid("b6c9cf0e-31a6-4a35-a932-05f1e58f4a01"), "A luxury hotel in the heart of Jerusalem.", 10, "Jerusalem Grand Hotel", "John Doe", "+970212345678", 4.7f, "123 Old City St." },
                    { new Guid("b3dd1dce-e79f-4d00-b5e3-f2d4d0a4f104"), new Guid("d3be3a21-8eac-48fa-a5f0-6c9e3c53ee03"), "Hotel with beautiful Nile river views.", 12, "Cairo Nile View", "Fatima Hassan", "+20212345678", 4.2f, "Nile Corniche" },
                    { new Guid("fbfc9c27-799b-4d7e-b12a-dbbdc914f105"), new Guid("f5f7c2b4-70a1-4b99-b6f2-8e416ab2de02"), "Conveniently located hotel in downtown Amman.", 7, "Amman City Center Hotel", "Leila Mansour", "+96265123457", 4.1f, "3 Downtown Rd." }
                });

            migrationBuilder.InsertData(
                table: "RoomTypes",
                columns: new[] { "Id", "Category", "HotelId", "PricePerNight" },
                values: new object[,]
                {
                    { new Guid("0af3d54f-214f-4c33-8a9e-2389329e0001"), "Single", new Guid("9d6d68f6-c6b0-41a9-b6f1-72d7d340b101"), 120m },
                    { new Guid("0e9a34a0-cf9e-4a41-b27c-96c3b5100008"), "Single", new Guid("69702fd3-fb46-4694-b5fc-3de9c8b5a103"), 130m },
                    { new Guid("0fa4010a-8c8a-4d8d-a6e7-3927c40a0002"), "Double", new Guid("9d6d68f6-c6b0-41a9-b6f1-72d7d340b101"), 200m },
                    { new Guid("1a734e0b-78c3-401e-b8b4-32d129110009"), "Suite", new Guid("04a5ffeb-9134-4097-9b4e-d4783c194102"), 400m },
                    { new Guid("2690e45c-015f-4c58-8a33-0db4f3120010"), "Double", new Guid("b3dd1dce-e79f-4d00-b5e3-f2d4d0a4f104"), 210m },
                    { new Guid("a2b8e135-84ef-4c34-83b1-1f68240b0003"), "Double", new Guid("04a5ffeb-9134-4097-9b4e-d4783c194102"), 180m },
                    { new Guid("b4fca5e3-2e10-4b85-9f98-0b16d50c0004"), "Suite", new Guid("69702fd3-fb46-4694-b5fc-3de9c8b5a103"), 350m },
                    { new Guid("cb0f3c22-7a44-44a7-9e43-7cc3e70d0005"), "Double", new Guid("b3dd1dce-e79f-4d00-b5e3-f2d4d0a4f104"), 220m },
                    { new Guid("de2d1f5a-6237-4e49-8b10-24de1e0e0006"), "Single", new Guid("fbfc9c27-799b-4d7e-b12a-dbbdc914f105"), 110m },
                    { new Guid("fbc8e6f6-0fca-4a34-8f9d-41b37a0f0007"), "Suite", new Guid("fbfc9c27-799b-4d7e-b12a-dbbdc914f105"), 380m }
                });

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "Id", "DiscountPercentage", "FromDate", "RoomTypeId", "ToDate" },
                values: new object[,]
                {
                    { new Guid("246cf7e5-8c03-4cc2-bb6a-7592a15c0005"), 12.5f, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("1a734e0b-78c3-401e-b8b4-32d129110009"), new DateTime(2025, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("342e5362-4a61-4d11-a7e7-6b01b47e0001"), 10f, new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("0af3d54f-214f-4c33-8a9e-2389329e0001"), new DateTime(2025, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("7851f29b-6d7a-4f7c-83ea-4f1b8c9c0004"), 20f, new DateTime(2025, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fbc8e6f6-0fca-4a34-8f9d-41b37a0f0007"), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("cb51a724-e4ca-46e9-9e3d-d59f3f770002"), 15f, new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("b4fca5e3-2e10-4b85-9f98-0b16d50c0004"), new DateTime(2025, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("cfe7a21e-344d-4ae3-8a62-7cc89b710003"), 5f, new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("cb0f3c22-7a44-44a7-9e43-7cc3e70d0005"), new DateTime(2025, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "AdultsCapacity", "ChildrenCapacity", "Rating", "RoomTypeId", "View" },
                values: new object[,]
                {
                    { new Guid("10cdbbe9-1e91-4dc5-94e5-cfb6fce5c607"), 4, 2, 4f, new Guid("cb0f3c22-7a44-44a7-9e43-7cc3e70d0005"), "Garden View" },
                    { new Guid("1d2cbcb0-6727-4d3e-8c90-1c7c7e48f482"), 3, 2, 4.2f, new Guid("0fa4010a-8c8a-4d8d-a6e7-3927c40a0002"), "Mountain View" },
                    { new Guid("99d8eb70-2190-4238-9f00-22f6e5b5a505"), 2, 2, 4.9f, new Guid("b4fca5e3-2e10-4b85-9f98-0b16d50c0004"), "Pool View" },
                    { new Guid("b03c67e0-3c3c-4a24-9fa0-9632d693ab01"), 2, 1, 4.5f, new Guid("0af3d54f-214f-4c33-8a9e-2389329e0001"), "Sea View" }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "AdultsCapacity", "Rating", "RoomTypeId", "View" },
                values: new object[] { new Guid("e47fcdf4-6355-4ea3-a33f-59ff56ad1f03"), 1, 3.8f, new Guid("a2b8e135-84ef-4c34-83b1-1f68240b0003"), "City View" });
        }
    }
}
