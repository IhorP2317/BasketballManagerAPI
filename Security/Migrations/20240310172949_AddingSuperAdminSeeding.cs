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
                    { new Guid("3d5b3817-cbe4-4ad3-90be-b7a6b4c40868"), "1bd76001-9027-4aff-bfb7-546494fd30e1", "User", "USER" },
                    { new Guid("cc03dd56-1825-454a-8453-997df48d036e"), "e416fede-f9db-44e0-afca-c1a59fbb214d", "Admin", "ADMIN" },
                    { new Guid("dc7c93de-93d5-4ef6-8d02-0901f4089ead"), "dc7c93de-93d5-4ef6-8d02-0901f4089ead", "SuperAdmin", "SUPERADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("226b1dad-0065-44c6-acef-93186e7cd0f2"), 0, "514d0db2-9519-4521-8b3f-585a278a53b4", "mrsplash2356@gmail.com", true, "Ihor", "Paranchuk", false, null, "MRSPLASH2356@GMAIL.COM", "IPVSPLASH1117@GMAIL.COM", "AQAAAAIAAYagAAAAEA6XOA6Giaarq51bg0Jcc9Y+9VtnCDrIfWdt/Xiskn8xNOHUtPWP3MdHzHdk6PaLCA==", null, false, null, "9b1b5eae-0a98-40b0-9607-bbedb9f4ef81", false, "ipvsplash1117@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("dc7c93de-93d5-4ef6-8d02-0901f4089ead"), new Guid("226b1dad-0065-44c6-acef-93186e7cd0f2") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3d5b3817-cbe4-4ad3-90be-b7a6b4c40868"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("cc03dd56-1825-454a-8453-997df48d036e"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("dc7c93de-93d5-4ef6-8d02-0901f4089ead"), new Guid("226b1dad-0065-44c6-acef-93186e7cd0f2") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("dc7c93de-93d5-4ef6-8d02-0901f4089ead"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("226b1dad-0065-44c6-acef-93186e7cd0f2"));

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
