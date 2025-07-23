using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Real_Estatae_Project.Migrations
{
    /// <inheritdoc />
    public partial class stripeAccountnullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addvertisements_AspNetUsers_userId",
                table: "Addvertisements");

            migrationBuilder.DropForeignKey(
                name: "FK_Addvertisements_Units_unitId",
                table: "Addvertisements");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_userId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_CommunityPosts_communityPostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Communities_AspNetUsers_ownerId",
                table: "Communities");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityPosts_AspNetUsers_userId",
                table: "CommunityPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityPosts_Communities_communityId",
                table: "CommunityPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_Maintenances_Units_unitId",
                table: "Maintenances");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_userId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_UserId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Rents_RentId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reacts_AspNetUsers_UserId",
                table: "Reacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Reacts_CommunityPosts_PostId",
                table: "Reacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Rents_Units_unitId",
                table: "Rents");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Appointments_appointmentId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_userId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Communities_communityId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Communities_communityId",
                table: "Units");

            migrationBuilder.DropForeignKey(
                name: "FK_VerificationCodes_AspNetUsers_userId",
                table: "VerificationCodes");

            migrationBuilder.AlterColumn<string>(
                name: "StripeAccountId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Addvertisements_AspNetUsers_userId",
                table: "Addvertisements",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Addvertisements_Units_unitId",
                table: "Addvertisements",
                column: "unitId",
                principalTable: "Units",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_userId",
                table: "Comments",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_CommunityPosts_communityPostId",
                table: "Comments",
                column: "communityPostId",
                principalTable: "CommunityPosts",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_AspNetUsers_ownerId",
                table: "Communities",
                column: "ownerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityPosts_AspNetUsers_userId",
                table: "CommunityPosts",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityPosts_Communities_communityId",
                table: "CommunityPosts",
                column: "communityId",
                principalTable: "Communities",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenances_Units_unitId",
                table: "Maintenances",
                column: "unitId",
                principalTable: "Units",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_userId",
                table: "Notifications",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_UserId",
                table: "Payments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Rents_RentId",
                table: "Payments",
                column: "RentId",
                principalTable: "Rents",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Reacts_AspNetUsers_UserId",
                table: "Reacts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Reacts_CommunityPosts_PostId",
                table: "Reacts",
                column: "PostId",
                principalTable: "CommunityPosts",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Rents_Units_unitId",
                table: "Rents",
                column: "unitId",
                principalTable: "Units",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Appointments_appointmentId",
                table: "Reservations",
                column: "appointmentId",
                principalTable: "Appointments",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_userId",
                table: "Reviews",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Communities_communityId",
                table: "Reviews",
                column: "communityId",
                principalTable: "Communities",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Communities_communityId",
                table: "Units",
                column: "communityId",
                principalTable: "Communities",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_VerificationCodes_AspNetUsers_userId",
                table: "VerificationCodes",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addvertisements_AspNetUsers_userId",
                table: "Addvertisements");

            migrationBuilder.DropForeignKey(
                name: "FK_Addvertisements_Units_unitId",
                table: "Addvertisements");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_userId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_CommunityPosts_communityPostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Communities_AspNetUsers_ownerId",
                table: "Communities");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityPosts_AspNetUsers_userId",
                table: "CommunityPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityPosts_Communities_communityId",
                table: "CommunityPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_Maintenances_Units_unitId",
                table: "Maintenances");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_userId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_UserId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Rents_RentId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reacts_AspNetUsers_UserId",
                table: "Reacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Reacts_CommunityPosts_PostId",
                table: "Reacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Rents_Units_unitId",
                table: "Rents");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Appointments_appointmentId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_userId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Communities_communityId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Communities_communityId",
                table: "Units");

            migrationBuilder.DropForeignKey(
                name: "FK_VerificationCodes_AspNetUsers_userId",
                table: "VerificationCodes");

            migrationBuilder.AlterColumn<string>(
                name: "StripeAccountId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Addvertisements_AspNetUsers_userId",
                table: "Addvertisements",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addvertisements_Units_unitId",
                table: "Addvertisements",
                column: "unitId",
                principalTable: "Units",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_userId",
                table: "Comments",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_CommunityPosts_communityPostId",
                table: "Comments",
                column: "communityPostId",
                principalTable: "CommunityPosts",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_AspNetUsers_ownerId",
                table: "Communities",
                column: "ownerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityPosts_AspNetUsers_userId",
                table: "CommunityPosts",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityPosts_Communities_communityId",
                table: "CommunityPosts",
                column: "communityId",
                principalTable: "Communities",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenances_Units_unitId",
                table: "Maintenances",
                column: "unitId",
                principalTable: "Units",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_userId",
                table: "Notifications",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_UserId",
                table: "Payments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Rents_RentId",
                table: "Payments",
                column: "RentId",
                principalTable: "Rents",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reacts_AspNetUsers_UserId",
                table: "Reacts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reacts_CommunityPosts_PostId",
                table: "Reacts",
                column: "PostId",
                principalTable: "CommunityPosts",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rents_Units_unitId",
                table: "Rents",
                column: "unitId",
                principalTable: "Units",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Appointments_appointmentId",
                table: "Reservations",
                column: "appointmentId",
                principalTable: "Appointments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_userId",
                table: "Reviews",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Communities_communityId",
                table: "Reviews",
                column: "communityId",
                principalTable: "Communities",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Communities_communityId",
                table: "Units",
                column: "communityId",
                principalTable: "Communities",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_VerificationCodes_AspNetUsers_userId",
                table: "VerificationCodes",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
