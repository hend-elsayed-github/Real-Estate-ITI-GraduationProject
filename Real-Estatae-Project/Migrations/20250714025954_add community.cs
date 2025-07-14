using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Real_Estatae_Project.Migrations
{
    /// <inheritdoc />
    public partial class addcommunity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityPosts_AspNetUsers_ApplicationUserId",
                table: "CommunityPosts");

            migrationBuilder.DropIndex(
                name: "IX_CommunityPosts_ApplicationUserId",
                table: "CommunityPosts");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "CommunityPosts");

            migrationBuilder.AddColumn<string>(
                name: "image",
                table: "CommunityPosts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "CommunityPosts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Reacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reacts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Reacts_CommunityPosts_PostId",
                        column: x => x.PostId,
                        principalTable: "CommunityPosts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunityPosts_userId",
                table: "CommunityPosts",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Reacts_PostId",
                table: "Reacts",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Reacts_UserId",
                table: "Reacts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityPosts_AspNetUsers_userId",
                table: "CommunityPosts",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityPosts_AspNetUsers_userId",
                table: "CommunityPosts");

            migrationBuilder.DropTable(
                name: "Reacts");

            migrationBuilder.DropIndex(
                name: "IX_CommunityPosts_userId",
                table: "CommunityPosts");

            migrationBuilder.DropColumn(
                name: "image",
                table: "CommunityPosts");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "CommunityPosts");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "CommunityPosts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommunityPosts_ApplicationUserId",
                table: "CommunityPosts",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityPosts_AspNetUsers_ApplicationUserId",
                table: "CommunityPosts",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
