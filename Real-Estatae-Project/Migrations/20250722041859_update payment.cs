using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Real_Estatae_Project.Migrations
{
    /// <inheritdoc />
    public partial class updatepayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_userId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_BankAccounts_bankAccountId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentMethods_paymentMethodId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropIndex(
                name: "IX_Payments_bankAccountId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_paymentMethodId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "bankAccountId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "paymentMethodId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "type",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Payments",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "amount",
                table: "Payments",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Payments",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_userId",
                table: "Payments",
                newName: "IX_Payments_UserId");

            migrationBuilder.AddColumn<string>(
                name: "CardBrand",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardLast4",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripePaymentIntentId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StripePaymentMethodId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StripeAccountId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_UserId",
                table: "Payments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_UserId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CardBrand",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CardLast4",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "StripePaymentIntentId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "StripePaymentMethodId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "StripeAccountId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Payments",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Payments",
                newName: "amount");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Payments",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_UserId",
                table: "Payments",
                newName: "IX_Payments_userId");

            migrationBuilder.AddColumn<int>(
                name: "bankAccountId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "paymentMethodId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    accountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bankName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.id);
                    table.ForeignKey(
                        name: "FK_BankAccounts_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_bankAccountId",
                table: "Payments",
                column: "bankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_paymentMethodId",
                table: "Payments",
                column: "paymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_userId",
                table: "BankAccounts",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_userId",
                table: "Payments",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_BankAccounts_bankAccountId",
                table: "Payments",
                column: "bankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentMethods_paymentMethodId",
                table: "Payments",
                column: "paymentMethodId",
                principalTable: "PaymentMethods",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
