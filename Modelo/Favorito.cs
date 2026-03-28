namespace TechStoreApi.Modelo
{
	public class Favorito
	{
		public int Id { get; set; }
		public int UsuarioId { get; set; }
		public int ProductoId { get; set; }
		public Producto? Producto { get; set; } 
	}
}
