using Microsoft.EntityFrameworkCore.Migrations;

namespace TicketingSystemDAL.Migrations
{
    public partial class EventSeatPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prices_SeatsTypes_SeatTypeId",
                table: "Prices");

            migrationBuilder.DropIndex(
                name: "IX_Prices_SeatTypeId",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "SeatTypeId",
                table: "Prices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SeatTypeId",
                table: "Prices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Prices_SeatTypeId",
                table: "Prices",
                column: "SeatTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_SeatsTypes_SeatTypeId",
                table: "Prices",
                column: "SeatTypeId",
                principalTable: "SeatsTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
