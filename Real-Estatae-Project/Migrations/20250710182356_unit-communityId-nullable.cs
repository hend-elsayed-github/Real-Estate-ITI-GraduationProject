using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Real_Estatae_Project.Migrations
{
    /// <inheritdoc />
    public partial class unitcommunityIdnullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_Communities_communityId",
                table: "Units");

            migrationBuilder.AlterColumn<int>(
                name: "communityId",
                table: "Units",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Communities_communityId",
                table: "Units",
                column: "communityId",
                principalTable: "Communities",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_Communities_communityId",
                table: "Units");

            migrationBuilder.AlterColumn<int>(
                name: "communityId",
                table: "Units",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Communities_communityId",
                table: "Units",
                column: "communityId",
                principalTable: "Communities",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
