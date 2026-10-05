using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Excursiones.Data.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Excursiones",
                columns: table => new
                {
                    IdExcursion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreExcursion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaExcursion = table.Column<DateTime>(type: "Date", nullable: false),
                    Borrado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Excursiones", x => x.IdExcursion);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Excursiones");
        }
    }
}
