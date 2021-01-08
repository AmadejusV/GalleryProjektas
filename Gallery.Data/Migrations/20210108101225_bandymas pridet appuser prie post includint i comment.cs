using Microsoft.EntityFrameworkCore.Migrations;

namespace Gallery.Data.Migrations
{
    public partial class bandymaspridetappuserpriepostincludinticomment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PostId",
                table: "AspNetUsers",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Posts_PostId",
                table: "AspNetUsers",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Posts_PostId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PostId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "AspNetUsers");
        }
    }
}
