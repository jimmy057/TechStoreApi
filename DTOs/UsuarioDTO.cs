namespace TechStoreApi.DTOs
{
	public class UsuarioDTO
	{
		public int UsuarioId { get; set; }    
		public string Nombre { get; set; } = string.Empty; 
		public string Email { get; set; } = string.Empty;
		public string Token { get; set; } = string.Empty;
		public string Rol { get; set; }
	}
}
