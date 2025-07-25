using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Real_Estatae_Project.Migrations
{
    /// <inheritdoc />
    public partial class notifiatio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "title",
                table: "Notifications",
                newName: "sender");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "sender",
                table: "Notifications",
                newName: "title");
        }
    }
}
