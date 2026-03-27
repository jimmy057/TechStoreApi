namespace TechStoreApi.DTOs
{
	public class PedidoDTO
	{
		public int Id { get; set; }
		public DateTime Fecha { get; set; }
		public decimal Total { get; set; }
		public string Estado { get; set; } = string.Empty;
		public List<DetallePedidoDTO> Items { get; set; } = new();
	}
}
