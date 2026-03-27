namespace TechStoreApi.DTOs
{
	public class DetallePedidoDTO
	{
		public string ProductoNombre { get; set; } = string.Empty;
		public int Cantidad { get; set; }
		public decimal PrecioUnitario { get; set; }
	}
}
