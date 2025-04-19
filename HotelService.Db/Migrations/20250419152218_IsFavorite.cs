using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelService.Db.Migrations
{
    /// <inheritdoc />
    public partial class IsFavorite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFavorite",
                table: "FavoriteHotels",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFavorite",
                table: "FavoriteHotels");
        }
    }
}
