using System.ComponentModel.DataAnnotations;

namespace TechStoreApi.Modelo
{
	public class Usuario
	{
		public int Id { get; set; }
		public string NombreCompleto { get; set; } = string.Empty;
		[Required]
		public string Email { get; set; } = string.Empty;
		[Required]
		public string PasswordHash { get; set; } = string.Empty; 
		public string Rol { get; set; } = "Cliente"; 
		public DateTime FechaRegistro { get; set; } = DateTime.Now;
	}
}
