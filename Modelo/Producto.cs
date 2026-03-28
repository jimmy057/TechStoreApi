using System.ComponentModel.DataAnnotations;

namespace TechStoreApi.Modelo
{
	public class Producto
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Nombre { get; set; } = string.Empty;
		public string Marca { get; set; } = string.Empty;
		public string Descripcion { get; set; } = string.Empty;
		public decimal Precio { get; set; }
		public decimal? PrecioOferta { get; set; }
		public int Stock { get; set; }
		public string ImagenUrl { get; set; } = string.Empty;

		public int CategoriaId { get; set; }
		public Categoría? Categoria { get; set; }

		public string Procesador { get; set; } = string.Empty;
		public string RAM { get; set; } = string.Empty;
		public string Almacenamiento { get; set; } = string.Empty;

		public double Calificacion { get; set; }
		public DateTime FechaCreado { get; set; } = DateTime.Now;

		public List<ImagenProducto> Imagenes { get; set; } = new();
	}
}
