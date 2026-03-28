namespace TechStoreApi.Modelo
{
	public class Pedido
	{
		public int Id { get; set; }
		public DateTime Fecha { get; set; } = DateTime.UtcNow;
		public decimal Total { get; set; }
		public string Estado { get; set; } = "Pendiente";
		public string DireccionEnvio { get; set; } = string.Empty;
		public string MetodoPago { get; set; } = "Tarjeta";
		public string MetodoEnvio { get; set; } = "Estándar";
		public string? NumeroGuia { get; set; }

		public int UsuarioId { get; set; }
		public Usuario? Usuario { get; set; }
		public List<DetallePedido> Detalles { get; set; } = new();
	}
}
