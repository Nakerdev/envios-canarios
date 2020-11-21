using Microsoft.EntityFrameworkCore.Migrations;

namespace CanaryDeliveries.PurchaseApplication.DbContext.Migrations
{
    public partial class CreatedPurchaseApplicationsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PurchaseApplicationId",
                table: "Products",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PurchaseApplications",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ClientId = table.Column<string>(nullable: false),
                    AdditionalInformation = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseApplications_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_PurchaseApplicationId",
                table: "Products",
                column: "PurchaseApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseApplications_ClientId",
                table: "PurchaseApplications",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_PurchaseApplications_PurchaseApplicationId",
                table: "Products",
                column: "PurchaseApplicationId",
                principalTable: "PurchaseApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_PurchaseApplications_PurchaseApplicationId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "PurchaseApplications");

            migrationBuilder.DropIndex(
                name: "IX_Products_PurchaseApplicationId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PurchaseApplicationId",
                table: "Products");
        }
    }
}
