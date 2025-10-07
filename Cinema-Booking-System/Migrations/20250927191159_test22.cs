using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinema_Booking_System.Migrations
{
    /// <inheritdoc />
    public partial class test22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "ApplicationUser",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "ApplicationUser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "ApplicationUser");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "ApplicationUser",
                newName: "FullName");
        }
    }
}
