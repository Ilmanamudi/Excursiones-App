using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Excursiones.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregarImagenExcursion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                table: "Excursiones",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                table: "Excursiones");
        }
    }
}
