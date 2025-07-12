using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Real_Estatae_Project.Migrations
{
    /// <inheritdoc />
    public partial class addingcitytounit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "Units",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "city",
                table: "Units");
        }
    }
}
