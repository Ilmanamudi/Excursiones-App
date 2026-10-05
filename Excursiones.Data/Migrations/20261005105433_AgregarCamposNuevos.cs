using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Excursiones.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCamposNuevos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cupo",
                table: "Excursiones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Excursiones",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DuracionDias",
                table: "Excursiones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdEstado",
                table: "Excursiones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Precio",
                table: "Excursiones",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "EstadosExcursion",
                columns: table => new
                {
                    IdEstado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreEstado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosExcursion", x => x.IdEstado);
                });

            migrationBuilder.Sql("INSERT INTO EstadosExcursion (NombreEstado) VALUES ('Programada'), ('Completada'), ('Cancelada');");

            migrationBuilder.Sql("UPDATE Excursiones SET IdEstado = 1;");

            migrationBuilder.CreateIndex(
                name: "IX_Excursiones_IdEstado",
                table: "Excursiones",
                column: "IdEstado");

            migrationBuilder.AddForeignKey(
                name: "FK_Excursiones_EstadosExcursion_IdEstado",
                table: "Excursiones",
                column: "IdEstado",
                principalTable: "EstadosExcursion",
                principalColumn: "IdEstado",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Excursiones_EstadosExcursion_IdEstado",
                table: "Excursiones");

            migrationBuilder.DropTable(
                name: "EstadosExcursion");

            migrationBuilder.DropIndex(
                name: "IX_Excursiones_IdEstado",
                table: "Excursiones");

            migrationBuilder.DropColumn(
                name: "Cupo",
                table: "Excursiones");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Excursiones");

            migrationBuilder.DropColumn(
                name: "DuracionDias",
                table: "Excursiones");

            migrationBuilder.DropColumn(
                name: "IdEstado",
                table: "Excursiones");

            migrationBuilder.DropColumn(
                name: "Precio",
                table: "Excursiones");
        }
    }
}
