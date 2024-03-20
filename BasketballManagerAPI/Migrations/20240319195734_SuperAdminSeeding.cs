using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasketballManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class SuperAdminSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Balance", "CreatedById", "CreatedTime", "Email", "EmailConfirmed", "FirstName", "LastName", "ModifiedById", "ModifiedTime", "PhotoPath", "Role" },
                values: new object[] { new Guid("226b1dad-0065-44c6-acef-93186e7cd0f2"), 0m, new Guid("00000000-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "mrsplash2356@gmail.com", true, "Ihor", "Paranchuk", null, null, null, 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("226b1dad-0065-44c6-acef-93186e7cd0f2"));
        }
    }
}
