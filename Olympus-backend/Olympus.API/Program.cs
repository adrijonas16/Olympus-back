using CapaDatos.DataContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Olympus.API.Extensions;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Production")
    ?? throw new InvalidOperationException("Connection string 'Production' not found.");

//var connectionString = builder.Environment.IsDevelopment()
//    ? builder.Configuration.GetConnectionString("Development")
//    : builder.Configuration.GetConnectionString("Production");

builder.Services.AddDbContext<OlympusContext>(options =>
    options.UseSqlServer(connectionString)
);


// 🔹 Agregar repositorios
builder.Services.AgregarServiciosAplicacion();

// 🔹 Configurar autenticación con JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddDefaultAuthorizationPolicy();

// 🔹 Agregar controladores
builder.Services.AddControllers();

// 🔹 Configurar CORS para permitir frontend local
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://161.35.59.115:3000", // Producción
            "http://localhost:5173"      // Desarrollo
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🔹 Middleware de Swagger
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 🔹 Usar política CORS
app.UseCors("AllowFrontend");

// 🔹 Habilitar autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
