using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reclamoService.Infraestructura.Migrations
{
    /// <inheritdoc />
    public partial class AgregarColumnaSolucion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Solucion",
                table: "Reclamos",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Solucion",
                table: "Reclamos");
        }
    }
}
