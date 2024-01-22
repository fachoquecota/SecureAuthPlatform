using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace AuthUI.Controllers
{
    public class UsersController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IConfiguration configuration, ILogger<UsersController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (HttpContext.Request.Cookies.TryGetValue("jwt", out var token))
            {
                try
                {
                    var validationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"])),
                        ValidateIssuer = true, // Validar el emisor
                        ValidIssuer = _configuration["JwtSettings:Issuer"], // Emisor esperado
                        ValidateAudience = true, // Validar la audiencia
                        ValidAudience = _configuration["JwtSettings:Audience"], // Audiencia esperada
                        ClockSkew = TimeSpan.Zero // Ajusta según sea necesario
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                    // Validar la información del usuario
                    if (principal.Identity is not null && principal.Identity.IsAuthenticated)
                    {
                        var usernameClaim = principal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
                        if (string.IsNullOrEmpty(usernameClaim))
                        {
                            // Manejo si el claim necesario no está presente
                            _logger.LogWarning("Claim 'sub' no encontrado en el token");
                            return RedirectToAction("Index", "Home");
                        }

                        ViewBag.Username = usernameClaim;
                        return View(); // Mostrar la página protegida
                    }
                }
                catch (SecurityTokenExpiredException ex)
                {
                    _logger.LogWarning(ex, "Token expirado en UsersController");
                    return RedirectToAction("Index", "Home");
                }
                catch (SecurityTokenException ex)
                {
                    _logger.LogWarning(ex, "Token inválido en UsersController");
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error inesperado en UsersController");
                    return RedirectToAction("Index", "Home");
                }
            }

            // Si no hay token o no es válido, redirige al login
            return RedirectToAction("Index", "Home");
        }


        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction("Index", "Home");
        }
    }
}
