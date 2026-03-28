namespace TechStoreApi.DTOs
{
	public class UsuarioDTO
	{
		public int Id { get; set; }
		public string NombreCompleto { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Token { get; set; } = string.Empty; 
	}
}
