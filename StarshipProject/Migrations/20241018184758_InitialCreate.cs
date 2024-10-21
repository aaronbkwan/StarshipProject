using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StarshipProject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Starships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Model = table.Column<string>(type: "TEXT", nullable: false),
                    Manufacturer = table.Column<string>(type: "TEXT", nullable: false),
                    CostInCredits = table.Column<int>(type: "INTEGER", nullable: false),
                    Length = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxAtmospheringSpeed = table.Column<int>(type: "INTEGER", nullable: false),
                    Crew = table.Column<int>(type: "INTEGER", nullable: false),
                    Passengers = table.Column<int>(type: "INTEGER", nullable: false),
                    CargoCapacity = table.Column<int>(type: "INTEGER", nullable: false),
                    Consumables = table.Column<string>(type: "TEXT", nullable: false),
                    HyperdriveRating = table.Column<double>(type: "REAL", nullable: false),
                    MGLT = table.Column<int>(type: "INTEGER", nullable: false),
                    StarshipClass = table.Column<string>(type: "TEXT", nullable: false),
                    Pilots = table.Column<string>(type: "TEXT", nullable: false),
                    Films = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Edited = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Starships", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Starships");
        }
    }
}
