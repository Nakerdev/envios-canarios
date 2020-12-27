using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CanaryDeliveries.PurchaseApplication.DbContext.Migrations
{
    public partial class ExtendPurchaseApplicationTableWithRejectionFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RejectionDateTime",
                table: "PurchaseApplication_PurchaseApplications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "PurchaseApplication_PurchaseApplications",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectionDateTime",
                table: "PurchaseApplication_PurchaseApplications");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "PurchaseApplication_PurchaseApplications");
        }
    }
}
