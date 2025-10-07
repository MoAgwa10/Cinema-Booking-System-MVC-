using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinema_Booking_System.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Cinemas_Cinemaid",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_Cinemaid",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "Cinemaid",
                table: "Movies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cinemaid",
                table: "Movies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Cinemaid",
                table: "Movies",
                column: "Cinemaid");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Cinemas_Cinemaid",
                table: "Movies",
                column: "Cinemaid",
                principalTable: "Cinemas",
                principalColumn: "id");
        }
    }
}
