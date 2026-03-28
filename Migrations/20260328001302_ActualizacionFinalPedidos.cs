using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechStoreApi.Migrations
{
    /// <inheritdoc />
    public partial class ActualizacionFinalPedidos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DireccionEnvio",
                table: "Pedidos",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DireccionEnvio",
                table: "Pedidos");
        }
    }
}
