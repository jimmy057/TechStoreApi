namespace TechStoreApi.DTOs
{
	public class RegisterRequest
	{
		public string Nombre { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Clave { get; set; } = string.Empty; 
	}
}
