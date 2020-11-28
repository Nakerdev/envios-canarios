using Microsoft.EntityFrameworkCore.Migrations;

namespace CanaryDeliveries.PurchaseApplication.DbContext.Migrations
{
    public partial class CreatedPurchaseApplicationsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PurchaseApplicationId",
                table: "PurchaseApplication_Products",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PurchaseApplication_PurchaseApplications",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ClientId = table.Column<string>(nullable: false),
                    AdditionalInformation = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseApplication_PurchaseApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseApplication_PurchaseApplications_PurchaseApplicatio~",
                        column: x => x.ClientId,
                        principalTable: "PurchaseApplication_Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseApplication_Products_PurchaseApplicationId",
                table: "PurchaseApplication_Products",
                column: "PurchaseApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseApplication_PurchaseApplications_ClientId",
                table: "PurchaseApplication_PurchaseApplications",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseApplication_Products_PurchaseApplication_PurchaseAp~",
                table: "PurchaseApplication_Products",
                column: "PurchaseApplicationId",
                principalTable: "PurchaseApplication_PurchaseApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseApplication_Products_PurchaseApplication_PurchaseAp~",
                table: "PurchaseApplication_Products");

            migrationBuilder.DropTable(
                name: "PurchaseApplication_PurchaseApplications");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseApplication_Products_PurchaseApplicationId",
                table: "PurchaseApplication_Products");

            migrationBuilder.DropColumn(
                name: "PurchaseApplicationId",
                table: "PurchaseApplication_Products");
        }
    }
}
