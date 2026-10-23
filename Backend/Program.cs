using Forkly.Api.DAO;
using Forkly.Api.Interface;
using Forkly.Api.Models.Configuracion;
using Forkly.Api.Models.Users;
using Forkly.Api.Service.Auth;
using Forkly.Api.Service.Email;
using Forkly.Api.Service.Verificacion;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

//Conexion a la base de datos
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ForklyConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

// Add services to the container.

// Enums como texto ("VERIFICADA"); sin política de nombres para que coincidan con el frontend
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Inyeccion de dependencias
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IVerificacionService, VerificacionService>();
builder.Services.AddScoped<IEmailService, EmailService>();

//Correo (la contraseña del SMTP va en user-secrets: Email:Contrasena)
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection(EmailOptions.Seccion));

//Limite de reenvios del codigo de verificacion: 3 cada 10 minutos por usuario
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("reenvioCodigo", context => RateLimitPartition.GetFixedWindowLimiter(
        context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? context.Connection.RemoteIpAddress?.ToString() ?? "",
        _ => new FixedWindowRateLimiterOptions { PermitLimit = 3, Window = TimeSpan.FromMinutes(10) }));
    options.OnRejected = (context, token) => new ValueTask(context.HttpContext.Response.WriteAsJsonAsync(
        new { message = "Ya pediste varios códigos. Espera unos minutos antes de pedir otro." }, token));
});

//Gestion de usuarios
builder.Services.AddIdentity<Usuario, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
        // Los mismos caracteres que valida el formulario de registro del frontend
        options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789._-";
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddErrorDescriber<ErroresIdentityEs>();

// La clave va en user-secrets (dotnet user-secrets set "Jwt:Key" "..."), nunca en appsettings
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Falta la configuración Jwt:Key. Configúrala con dotnet user-secrets.");

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
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
else
{
    // En desarrollo el frontend llega por el proxy de Vite en http, así que no se redirige
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();
// Después de la autenticación para poder limitar por usuario
app.UseRateLimiter();

app.MapControllers();

app.Run();
