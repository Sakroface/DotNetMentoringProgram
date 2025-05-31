using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TicketingSystemDAL.Migrations
{
    public partial class CartPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Prices_PriceId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_PriceId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "PriceId",
                table: "Carts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PriceId",
                table: "Carts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Carts_PriceId",
                table: "Carts",
                column: "PriceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Prices_PriceId",
                table: "Carts",
                column: "PriceId",
                principalTable: "Prices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
