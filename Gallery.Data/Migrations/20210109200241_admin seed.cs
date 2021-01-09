using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gallery.Data.Migrations
{
    public partial class adminseed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { new Guid("b9b652f0-2f93-4c3d-a65b-ac53a1ba2bf3"), new Guid("b23075c4-e58a-458b-8933-f6913e4a55d0") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b23075c4-e58a-458b-8933-f6913e4a55d0"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b9b652f0-2f93-4c3d-a65b-ac53a1ba2bf3"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("d0f761b2-fe85-471e-914b-b26c46620403"), "27747190-7b7d-453d-ba7b-5bfa31119160", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("b1bd17f5-7ff9-4033-b0bb-4b71ab29fe3f"), 0, "2753310e-b58f-419e-bcb5-50d619ef4ed2", "igne@admin.com", true, false, null, "Adminas", "IGNE@ADMIN.COM", "IGNE@ADMIN.COM", "AQAAAAEAACcQAAAAEAlgwj8nK0YJlLN9vWhPrDweK0wc2Nh/299BrJGh6zlBkTfF5oNO8dBW6Xjd+vAgJA==", null, false, "2S3BIYUGVFUZY4FBPDZZ4354ZFCRBVUV", false, "igne@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { new Guid("b1bd17f5-7ff9-4033-b0bb-4b71ab29fe3f"), new Guid("d0f761b2-fe85-471e-914b-b26c46620403") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { new Guid("b1bd17f5-7ff9-4033-b0bb-4b71ab29fe3f"), new Guid("d0f761b2-fe85-471e-914b-b26c46620403") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d0f761b2-fe85-471e-914b-b26c46620403"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b1bd17f5-7ff9-4033-b0bb-4b71ab29fe3f"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("b23075c4-e58a-458b-8933-f6913e4a55d0"), "27747190-7b7d-453d-ba7b-5bfa31119160", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("b9b652f0-2f93-4c3d-a65b-ac53a1ba2bf3"), 0, "27747190-7b7d-453d-ba7b-5bfa31119160", "admin@admin.com", true, false, null, "Adminas", "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAEOpvPVTNsK5osJyR0T+4qh/+6m4CKrv7u+KH+rrB+ptHxAyVknaIysUmJm/UTPOhkw==", null, false, "IHDXOW62GL33UAOIJKMU6JBSKSBC63JJ", false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { new Guid("b9b652f0-2f93-4c3d-a65b-ac53a1ba2bf3"), new Guid("b23075c4-e58a-458b-8933-f6913e4a55d0") });
        }
    }
}
