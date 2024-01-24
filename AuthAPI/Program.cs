using AuthAPI.Business.Modules.Auth.Interfaces;
using AuthAPI.Business.Modules.Auth;
using AuthAPI.Business.Modules.Logs.Interfaces;
using AuthAPI.Business.Modules.Logs;
using AuthAPI.Business.Modules.Roles.Interfaces;
using AuthAPI.Business.Modules.Roles;
using AuthAPI.Data.Modules.Auth.Interfaces;
using AuthAPI.Data.Modules.Auth;
using AuthAPI.Data.Modules.Roles.Interfaces;
using AuthAPI.Data.Modules.Roles;
using AuthAPI.Data.Modules.Logs.Interfaces;
using AuthAPI.Data.Modules.Logs;
using AuthAPI.Data;
using AuthAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuración de JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = builder.Configuration["JwtSettings:SecretKey"];
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

// Agrega servicios al contenedor.
builder.Services.AddControllers();

// Configuración de Swagger
builder.Services.AddSwaggerGen();

// Registra tus servicios de negocio
builder.Services.AddScoped<IAuthB, AuthB>();
builder.Services.AddScoped<IRolesB, RolesB>();
builder.Services.AddScoped<ILogsB, LogsB>();

// Registra tus servicios de acceso a datos
builder.Services.AddScoped<IAuth, AuthSP>();
builder.Services.AddScoped<IRoles, RolesSP>();
builder.Services.AddScoped<ILogs, LogsSP>();

// Registra la clase Connection
builder.Services.AddTransient<Connection>();

// Registra TokenService
builder.Services.AddSingleton<TokenService>();

builder.Services.AddSingleton<TokenValidationService>();


var app = builder.Build();

// Configura el pipeline de solicitudes HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync("An unexpected error occurred");
    });
});
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();