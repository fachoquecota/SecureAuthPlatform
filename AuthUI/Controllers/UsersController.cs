using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AuthUI.Controllers
{
    public class UsersController : Controller
    {
        private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            if (HttpContext.Request.Cookies.TryGetValue("jwt", out var token))
            {
                try
                {
                    var palabra = _configuration["JwtSettings:SecretKey"];

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var validationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"])),
                        ValidateIssuer = false, // En producción deberías poner esto en true
                        ValidateAudience = false, // En producción deberías poner esto en true
                        ClockSkew = TimeSpan.Zero // Ajusta según sea necesario
                    };

                    var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                    var jwtToken = validatedToken as JwtSecurityToken;

                    if (jwtToken != null)
                    {
                        var usernameClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;
                        ViewBag.Username = usernameClaim;
                        return View(); // Mostrar la página protegida
                    }
                }
                catch (SecurityTokenExpiredException)
                {
                    // El token ha expirado
                    return RedirectToAction("Index", "Home");
                }
                catch (SecurityTokenException)
                {
                    // El token no es válido
                    return RedirectToAction("Index", "Home");
                }
            }

            // Si no hay token o no es válido, redirige al login
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Logout()
        {
            // Eliminar la cookie que contiene el token JWT
            Response.Cookies.Delete("jwt");

            // Redirigir a la página de inicio (Home/Index)
            return RedirectToAction("Index", "Home");
        }
    }
}
