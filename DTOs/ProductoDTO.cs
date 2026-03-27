namespace TechStoreApi.DTOs
{
	public class ProductoDTO
	{
		public int Id { get; set; }
		public string Nombre { get; set; } = string.Empty;
		public string Marca { get; set; } = string.Empty;
		public string Descripcion { get; set; } = string.Empty;
		public decimal Precio { get; set; }
		public decimal? PrecioOferta { get; set; }
		public string ImagenUrl { get; set; } = string.Empty;
		public string NombreCategoria { get; set; } = string.Empty; 
		public string Procesador { get; set; } = string.Empty;
		public string RAM { get; set; } = string.Empty;
		public double Calificacion { get; set; }
	}
}
