using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Real_Estatae_Project.Migrations
{
    /// <inheritdoc />
    public partial class billIdissue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_BillParent_Billid",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_BillParent_billId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_billId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_Billid",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Billid",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "billId",
                table: "Payments");

            migrationBuilder.CreateTable(
                name: "BillPayments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    paymentId = table.Column<int>(type: "int", nullable: false),
                    billId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillPayments", x => x.id);
                    table.ForeignKey(
                        name: "FK_BillPayments_BillParent_billId",
                        column: x => x.billId,
                        principalTable: "BillParent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BillPayments_Payments_paymentId",
                        column: x => x.paymentId,
                        principalTable: "Payments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BillPayments_billId",
                table: "BillPayments",
                column: "billId");

            migrationBuilder.CreateIndex(
                name: "IX_BillPayments_paymentId",
                table: "BillPayments",
                column: "paymentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillPayments");

            migrationBuilder.AddColumn<int>(
                name: "Billid",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "billId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_billId",
                table: "Payments",
                column: "billId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Billid",
                table: "Payments",
                column: "Billid",
                unique: true,
                filter: "[Billid] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_BillParent_Billid",
                table: "Payments",
                column: "Billid",
                principalTable: "BillParent",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_BillParent_billId",
                table: "Payments",
                column: "billId",
                principalTable: "BillParent",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
