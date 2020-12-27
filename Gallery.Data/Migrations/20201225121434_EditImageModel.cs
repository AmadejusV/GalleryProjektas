using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gallery.Data.Migrations
{
    public partial class EditImageModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Images",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Images");

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Images",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Images",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Images",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Images",
                table: "Images",
                column: "ImageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Images",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Images");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Images",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Images",
                table: "Images",
                column: "Id");
        }
    }
}
