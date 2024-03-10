using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Security.Migrations
{
    /// <inheritdoc />
    public partial class AddingSuperAdminSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("4f025378-4dd7-4d04-9299-6d42e2fea6cd"), "4f025378-4dd7-4d04-9299-6d42e2fea6cd", "SuperAdmin", "SUPERADMIN" },
                    { new Guid("9b9fd04f-208a-474c-8bef-c6759e7b46af"), "cec7dc53-1b6c-4c74-822f-5c544ef40027", "User", "USER" },
                    { new Guid("d73fecf9-3a55-4fb6-acbc-eb860d0bd022"), "e2a302a1-5e4d-46b3-83c6-6294aa508079", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("0aa8761a-1f16-4440-bcf3-9c17a5cee26f"), 0, "65d5db49-251b-4535-9100-c85b4fe403e5", "mrsplash2356@gmail.com", true, "Ihor", "Paranchuk", false, null, null, "IPVSPLASH1117@GMAIL.COM", "AQAAAAIAAYagAAAAENNcgL1skYh+AX/60FUc5fI//vkBoiEDB7xm+wzS79RGi7H+9N77gccVnfoj+Rau/w==", null, false, null, null, false, "ipvsplash1117@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("4f025378-4dd7-4d04-9299-6d42e2fea6cd"), new Guid("0aa8761a-1f16-4440-bcf3-9c17a5cee26f") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("9b9fd04f-208a-474c-8bef-c6759e7b46af"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d73fecf9-3a55-4fb6-acbc-eb860d0bd022"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("4f025378-4dd7-4d04-9299-6d42e2fea6cd"), new Guid("0aa8761a-1f16-4440-bcf3-9c17a5cee26f") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4f025378-4dd7-4d04-9299-6d42e2fea6cd"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("0aa8761a-1f16-4440-bcf3-9c17a5cee26f"));

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
    }
}
