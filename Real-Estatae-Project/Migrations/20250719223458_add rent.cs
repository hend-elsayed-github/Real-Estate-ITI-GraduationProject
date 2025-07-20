using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Real_Estatae_Project.Migrations
{
    /// <inheritdoc />
    public partial class addrent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Maintenances_BillParents_billId",
                table: "Maintenances");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_BillParents_billId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Rents_BillParents_billId",
                table: "Rents");

            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "BillParents");

            migrationBuilder.DropIndex(
                name: "IX_Rents_billId",
                table: "Rents");

            migrationBuilder.DropIndex(
                name: "IX_Rents_unitId",
                table: "Rents");

            migrationBuilder.DropIndex(
                name: "IX_Maintenances_billId",
                table: "Maintenances");

            migrationBuilder.DropColumn(
                name: "billId",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "paymentDate",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "billId",
                table: "Maintenances");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "Rents",
                newName: "Rentvalue");

            migrationBuilder.RenameColumn(
                name: "billId",
                table: "Payments",
                newName: "RentId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_billId",
                table: "Payments",
                newName: "IX_Payments_RentId");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Rents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateOnly>(
                name: "PaymentDate",
                table: "Payments",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.CreateIndex(
                name: "IX_Rents_unitId",
                table: "Rents",
                column: "unitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Rents_RentId",
                table: "Payments",
                column: "RentId",
                principalTable: "Rents",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Rents_RentId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Rents_unitId",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "Rentvalue",
                table: "Rents",
                newName: "value");

            migrationBuilder.RenameColumn(
                name: "RentId",
                table: "Payments",
                newName: "billId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_RentId",
                table: "Payments",
                newName: "IX_Payments_billId");

            migrationBuilder.AddColumn<int>(
                name: "billId",
                table: "Rents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "paymentDate",
                table: "Rents",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Rents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "billId",
                table: "Maintenances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BillParents",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    value = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillParents", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    billId = table.Column<int>(type: "int", nullable: false),
                    unitId = table.Column<int>(type: "int", nullable: false),
                    expirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    value = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.id);
                    table.ForeignKey(
                        name: "FK_Bills_BillParents_billId",
                        column: x => x.billId,
                        principalTable: "BillParents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Bills_Units_unitId",
                        column: x => x.unitId,
                        principalTable: "Units",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rents_billId",
                table: "Rents",
                column: "billId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rents_unitId",
                table: "Rents",
                column: "unitId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Maintenances_billId",
                table: "Maintenances",
                column: "billId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bills_billId",
                table: "Bills",
                column: "billId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bills_unitId",
                table: "Bills",
                column: "unitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenances_BillParents_billId",
                table: "Maintenances",
                column: "billId",
                principalTable: "BillParents",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_BillParents_billId",
                table: "Payments",
                column: "billId",
                principalTable: "BillParents",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Rents_BillParents_billId",
                table: "Rents",
                column: "billId",
                principalTable: "BillParents",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
