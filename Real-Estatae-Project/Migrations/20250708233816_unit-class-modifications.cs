using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Real_Estatae_Project.Migrations
{
    /// <inheritdoc />
    public partial class unitclassmodifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_AspNetUsers_userId",
                table: "Units");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Units",
                newName: "ownerId");

            migrationBuilder.RenameColumn(
                name: "image",
                table: "Units",
                newName: "image3");

            migrationBuilder.RenameIndex(
                name: "IX_Units_userId",
                table: "Units",
                newName: "IX_Units_ownerId");

            migrationBuilder.AddColumn<string>(
                name: "image1",
                table: "Units",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "image2",
                table: "Units",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "renterId",
                table: "Units",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_renterId",
                table: "Units",
                column: "renterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_AspNetUsers_ownerId",
                table: "Units",
                column: "ownerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_AspNetUsers_renterId",
                table: "Units",
                column: "renterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_AspNetUsers_ownerId",
                table: "Units");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_AspNetUsers_renterId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_renterId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "image1",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "image2",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "renterId",
                table: "Units");

            migrationBuilder.RenameColumn(
                name: "ownerId",
                table: "Units",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "image3",
                table: "Units",
                newName: "image");

            migrationBuilder.RenameIndex(
                name: "IX_Units_ownerId",
                table: "Units",
                newName: "IX_Units_userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_AspNetUsers_userId",
                table: "Units",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
