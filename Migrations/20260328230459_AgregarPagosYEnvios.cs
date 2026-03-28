using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechStoreApi.Migrations
{
    /// <inheritdoc />
    public partial class AgregarPagosYEnvios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FechaPedido",
                table: "Pedidos",
                newName: "Fecha");

            migrationBuilder.AddColumn<string>(
                name: "MetodoEnvio",
                table: "Pedidos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MetodoPago",
                table: "Pedidos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NumeroGuia",
                table: "Pedidos",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MetodoEnvio",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "MetodoPago",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "NumeroGuia",
                table: "Pedidos");

            migrationBuilder.RenameColumn(
                name: "Fecha",
                table: "Pedidos",
                newName: "FechaPedido");
        }
    }
}
