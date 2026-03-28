namespace TechStoreApi.Modelo
{
	public class Pedido
	{
		public int Id { get; set; }
		public DateTime FechaPedido { get; set; } = DateTime.UtcNow;
		public decimal Total { get; set; }
		public string Estado { get; set; } = "Pendiente";
		public string DireccionEnvio { get; set; } = string.Empty; 

		public int UsuarioId { get; set; }
		public Usuario? Usuario { get; set; }

		public List<DetallePedido> Detalles { get; set; } = new();
	}
}
