using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StarshipProject.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCrewAndSpeedToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MaxAtmospheringSpeed",
                table: "Starships",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MaxAtmospheringSpeed",
                table: "Starships",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
