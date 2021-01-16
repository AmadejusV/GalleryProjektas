using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gallery.Data.Migrations
{
    public partial class backtolocalDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { new Guid("f468c2f6-e932-4f04-a53a-8eaea5602079"), new Guid("d836f9a3-270e-4441-a089-5753c45849e4") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d836f9a3-270e-4441-a089-5753c45849e4"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f468c2f6-e932-4f04-a53a-8eaea5602079"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("90093a22-f3ee-4adb-b7f9-ce37acc18a7a"), "27747190-7b7d-453d-ba7b-5bfa31119160", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("deceee7f-4c18-499c-b436-d7be9b319e89"), 0, "2753310e-b58f-419e-bcb5-50d619ef4ed2", "igne@admin.com", true, false, null, "Admin", "IGNE@ADMIN.COM", "IGNE@ADMIN.COM", "AQAAAAEAACcQAAAAEAlgwj8nK0YJlLN9vWhPrDweK0wc2Nh/299BrJGh6zlBkTfF5oNO8dBW6Xjd+vAgJA==", null, false, "2S3BIYUGVFUZY4FBPDZZ4354ZFCRBVUV", false, "igne@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { new Guid("deceee7f-4c18-499c-b436-d7be9b319e89"), new Guid("90093a22-f3ee-4adb-b7f9-ce37acc18a7a") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { new Guid("deceee7f-4c18-499c-b436-d7be9b319e89"), new Guid("90093a22-f3ee-4adb-b7f9-ce37acc18a7a") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("90093a22-f3ee-4adb-b7f9-ce37acc18a7a"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("deceee7f-4c18-499c-b436-d7be9b319e89"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("d836f9a3-270e-4441-a089-5753c45849e4"), "27747190-7b7d-453d-ba7b-5bfa31119160", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("f468c2f6-e932-4f04-a53a-8eaea5602079"), 0, "2753310e-b58f-419e-bcb5-50d619ef4ed2", "igne@admin.com", true, false, null, "Admin", "IGNE@ADMIN.COM", "IGNE@ADMIN.COM", "AQAAAAEAACcQAAAAEAlgwj8nK0YJlLN9vWhPrDweK0wc2Nh/299BrJGh6zlBkTfF5oNO8dBW6Xjd+vAgJA==", null, false, "2S3BIYUGVFUZY4FBPDZZ4354ZFCRBVUV", false, "igne@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { new Guid("f468c2f6-e932-4f04-a53a-8eaea5602079"), new Guid("d836f9a3-270e-4441-a089-5753c45849e4") });
        }
    }
}
