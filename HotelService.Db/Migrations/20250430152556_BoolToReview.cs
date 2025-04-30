using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelService.Db.Migrations
{
    /// <inheritdoc />
    public partial class BoolToReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Reviews",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Reviews");
        }
    }
}
