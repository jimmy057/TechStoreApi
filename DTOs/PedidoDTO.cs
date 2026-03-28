namespace TechStoreApi.DTOs
{
	public class PedidoDto
	{
		public int UsuarioId { get; set; }
		public decimal Total { get; set; }
		public string DireccionEnvio { get; set; } = string.Empty;

		public string MetodoPago { get; set; } = string.Empty;
		public string MetodoEnvio { get; set; } = string.Empty;

		public List<DetallePedidoDTO> Detalles { get; set; } = new();
	}
}
