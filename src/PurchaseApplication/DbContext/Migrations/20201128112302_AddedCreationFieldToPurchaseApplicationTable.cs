using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CanaryDeliveries.PurchaseApplication.DbContext.Migrations
{
    public partial class AddedCreationFieldToPurchaseApplicationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDateTime",
                table: "PurchaseApplication_PurchaseApplications",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDateTime",
                table: "PurchaseApplication_PurchaseApplications");
        }
    }
}
