using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Real_Estatae_Project.Migrations
{
    /// <inheritdoc />
    public partial class removefk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Addvertisements_advertisementId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AspNetUsers_ownerId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Appointments_appointmentId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_appointmentId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_advertisementId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_ownerId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "appointmentId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "advertisementId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "ownerId",
                table: "Appointments");

            migrationBuilder.AddColumn<int>(
                name: "Addvertisementid",
                table: "Appointments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_Addvertisementid",
                table: "Appointments",
                column: "Addvertisementid");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ApplicationUserId",
                table: "Appointments",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Addvertisements_Addvertisementid",
                table: "Appointments",
                column: "Addvertisementid",
                principalTable: "Addvertisements",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AspNetUsers_ApplicationUserId",
                table: "Appointments",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Addvertisements_Addvertisementid",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AspNetUsers_ApplicationUserId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_Addvertisementid",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_ApplicationUserId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Addvertisementid",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Appointments");

            migrationBuilder.AddColumn<int>(
                name: "appointmentId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "advertisementId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ownerId",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_appointmentId",
                table: "Reservations",
                column: "appointmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_advertisementId",
                table: "Appointments",
                column: "advertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ownerId",
                table: "Appointments",
                column: "ownerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Addvertisements_advertisementId",
                table: "Appointments",
                column: "advertisementId",
                principalTable: "Addvertisements",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AspNetUsers_ownerId",
                table: "Appointments",
                column: "ownerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Appointments_appointmentId",
                table: "Reservations",
                column: "appointmentId",
                principalTable: "Appointments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
