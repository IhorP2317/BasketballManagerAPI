using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Security.Migrations
{
    /// <inheritdoc />
    public partial class AddingRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("58550f3f-b2e2-43c3-853b-1705e07fee10"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e6b40403-41c4-46d0-876f-728bd336a0e8"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("fbdc8d8f-6a55-48a7-b60f-0165aeb42290"));

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("0a6624da-7bff-4755-a0f5-581adb304708"), null, "User", "USER" },
                    { new Guid("1846c596-15ea-4095-8aa5-734ed6a79d0a"), null, "SuperAdmin", "SUPERADMIN" },
                    { new Guid("45414ab6-9dc3-42fa-b89c-e8a77272cecf"), null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0a6624da-7bff-4755-a0f5-581adb304708"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1846c596-15ea-4095-8aa5-734ed6a79d0a"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("45414ab6-9dc3-42fa-b89c-e8a77272cecf"));

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("58550f3f-b2e2-43c3-853b-1705e07fee10"), null, "User", "USER" },
                    { new Guid("e6b40403-41c4-46d0-876f-728bd336a0e8"), null, "SuperAdmin", "SUPERADMIN" },
                    { new Guid("fbdc8d8f-6a55-48a7-b60f-0165aeb42290"), null, "Admin", "ADMIN" }
                });
        }
    }
}
