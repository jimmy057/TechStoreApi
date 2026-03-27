using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TechStoreApi.Data;
using TechStoreApi.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar Entity Framework con SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Registrar el Servicio de JWT (Inyección de Dependencias)
builder.Services.AddScoped<JwtService>();

// 3. Configurar Autenticación JWT
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "Clave_Super_Secreta_TechStore_2026");
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "TechStoreApi",
		ValidAudience = builder.Configuration["Jwt:Audience"] ?? "TechStoreAndroidClient",
		IssuerSigningKey = new SymmetricSecurityKey(key)
	};
});

// 4. Habilitar CORS (Vital para que Android Studio pueda conectar)
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAndroidApp", policy =>
	{
		policy.AllowAnyOrigin()
			  .AllowAnyMethod()
			  .AllowAnyHeader();
	});
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 5. Configurar Swagger para soportar el candado de JWT
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "TechStore API", Version = "v1" });
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer"
	});
	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
			},
			Array.Empty<string>()
		}
	});
});

var app = builder.Build();

// 6. Pipeline de la Aplicación
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// Orden importante: CORS -> Auth -> Authorization
app.UseCors("AllowAndroidApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
