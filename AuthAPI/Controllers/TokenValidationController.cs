using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using AuthAPI.Services;
using AuthAPI.Models;

namespace AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenValidationController : ControllerBase
    {
        private readonly TokenValidationService _tokenValidationService;

        public TokenValidationController(TokenValidationService tokenValidationService)
        {
            _tokenValidationService = tokenValidationService;
        }

        [HttpPost("validate")]
        public IActionResult ValidateToken([FromBody] TokenValidationRequest request)
        {
            string token = request.Token;

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is required.");
            }

            try
            {
                var username = _tokenValidationService.GetUsernameFromToken(token);
                var modules = new[] { "Module1", "Module2", "Module3" }; // Lista ficticia de módulos

                // Token is valid
                return Ok(new { IsValid = true, Username = username, Modules = modules });
            }
            catch
            {
                // Token is invalid
                return Ok(new { IsValid = false, Username = (string)null, Modules = new string[] { } });
            }
        }
    }
}