using System.ComponentModel.DataAnnotations;

namespace TechStoreApi.Modelo
{
	public class ImagenProducto
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int ProductoId { get; set; }

		[Required]
		public string Url { get; set; } = string.Empty;

		public Producto? Producto { get; set; }
	}
}
