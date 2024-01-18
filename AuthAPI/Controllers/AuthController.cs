using Microsoft.AspNetCore.Mvc;
using AuthAPI.Business.Modules.Auth.Interfaces;
using AuthAPI.Models;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System;
using AuthAPI.Services;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthB _authService;
    private readonly TokenService _tokenService;

    public AuthController(IAuthB authService, TokenService tokenService)
    {
        _authService = authService;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserModel user)
    {
        await _authService.CreateUserAsync(user);
        // Opcionalmente, genera y devuelve un token aquí si lo necesitas
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        var user = await _authService.AuthenticateAsync(login.Username, login.Password);
        if (user == null)
            return Unauthorized();

        var token = _tokenService.GenerateJwtToken(user);
        return Ok(new { Token = token });
    }  
}
