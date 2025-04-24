using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelService.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationIdToReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "CreatedAt",
                table: "Reviews",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<int>(
                name: "ReservationId",
                table: "Reviews",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "Reviews");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Reviews",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}
