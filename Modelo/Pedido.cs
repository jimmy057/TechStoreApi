namespace TechStoreApi.Modelo
{
	public class Pedido
	{
		public int Id { get; set; }
		public DateTime FechaPedido { get; set; } = DateTime.Now;
		public decimal Total { get; set; }
		public string Estado { get; set; } = "Pendiente"; // Pendiente, Enviado, Entregado

		// Relación con Usuario
		public int UsuarioId { get; set; }
		public Usuario? Usuario { get; set; }

		// Detalle de los productos comprados
		public List<DetallePedido> Detalles { get; set; } = new();
	}
}
