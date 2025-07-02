using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TicketingSystemDAL.Migrations
{
    public partial class EventSeatPriceRefactoring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prices_EventSeats_EventSeatId",
                table: "Prices");

            migrationBuilder.RenameColumn(
                name: "EventSeatId",
                table: "Prices",
                newName: "SeatTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Prices_EventSeatId",
                table: "Prices",
                newName: "IX_Prices_SeatTypeId");

            migrationBuilder.AddColumn<Guid>(
                name: "PriceId",
                table: "EventSeats",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EventSeats_PriceId",
                table: "EventSeats",
                column: "PriceId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventSeats_Prices_PriceId",
                table: "EventSeats",
                column: "PriceId",
                principalTable: "Prices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_SeatsTypes_SeatTypeId",
                table: "Prices",
                column: "SeatTypeId",
                principalTable: "SeatsTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventSeats_Prices_PriceId",
                table: "EventSeats");

            migrationBuilder.DropForeignKey(
                name: "FK_Prices_SeatsTypes_SeatTypeId",
                table: "Prices");

            migrationBuilder.DropIndex(
                name: "IX_EventSeats_PriceId",
                table: "EventSeats");

            migrationBuilder.DropColumn(
                name: "PriceId",
                table: "EventSeats");

            migrationBuilder.RenameColumn(
                name: "SeatTypeId",
                table: "Prices",
                newName: "EventSeatId");

            migrationBuilder.RenameIndex(
                name: "IX_Prices_SeatTypeId",
                table: "Prices",
                newName: "IX_Prices_EventSeatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_EventSeats_EventSeatId",
                table: "Prices",
                column: "EventSeatId",
                principalTable: "EventSeats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
