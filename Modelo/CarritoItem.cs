using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechStoreApi.Modelo
{
	public class CarritoItems
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int UsuarioId { get; set; }

		[Required]
		public int ProductoId { get; set; }

		[Required]
		[Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1")]
		public int Cantidad { get; set; }

		[ForeignKey("ProductoId")]
		public Producto? Producto { get; set; }
	}
}
