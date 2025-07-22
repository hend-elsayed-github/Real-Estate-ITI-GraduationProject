using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Real_Estatae_Project.Migrations
{
    /// <inheritdoc />
    public partial class reviewscommunity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Units_unitId",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "unitId",
                table: "Reviews",
                newName: "communityId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_unitId",
                table: "Reviews",
                newName: "IX_Reviews_communityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Communities_communityId",
                table: "Reviews",
                column: "communityId",
                principalTable: "Communities",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Communities_communityId",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "communityId",
                table: "Reviews",
                newName: "unitId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_communityId",
                table: "Reviews",
                newName: "IX_Reviews_unitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Units_unitId",
                table: "Reviews",
                column: "unitId",
                principalTable: "Units",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
