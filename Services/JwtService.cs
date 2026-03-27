using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TechStoreApi.Modelo;

namespace TechStoreApi.Services
{
	public class JwtService
	{
		private readonly IConfiguration _config;

		public JwtService(IConfiguration config)
		{
			_config = config;
		}

		public string GenerarToken(Usuario usuario)
		{
			var key = _config["Jwt:Key"]
				?? throw new Exception("La clave JWT no está configurada en appsettings.json");

			var issuer = _config["Jwt:Issuer"] ?? "TechStoreApi";
			var audience = _config["Jwt:Audience"] ?? "TechStoreAndroidClient";

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
				new Claim(ClaimTypes.Name, usuario.NombreCompleto),
				new Claim(ClaimTypes.Email, usuario.Email),
				new Claim(ClaimTypes.Role, usuario.Rol), 
                new Claim("uid", usuario.Id.ToString()) 
            };

			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: issuer,
				audience: audience,
				claims: claims,
				expires: DateTime.UtcNow.AddHours(24), 
				signingCredentials: credentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
