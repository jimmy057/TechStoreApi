using System.ComponentModel.DataAnnotations;

namespace TechStoreApi.Modelo
{
	public class Categoría
	{
		public int Id { get; set; }
		[Required]
		public string Nombre { get; set; } = string.Empty;
		public string ImagenIconoUrl { get; set; } = string.Empty; 

		public List<Producto> Productos { get; set; } = new();
	}
}
