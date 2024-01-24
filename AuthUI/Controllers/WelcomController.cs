using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using AuthUI.Models.Response;
using AuthUI.Models.Request;

namespace AuthUI.Controllers
{
    public class WelcomController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public WelcomController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Request.Cookies["jwt"];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var tokenValidationRequest = new TokenValidationRequest { Token = token };
                var response = await httpClient.PostAsJsonAsync("http://localhost:5101/api/TokenValidation/validate", tokenValidationRequest);

                if (response.IsSuccessStatusCode)
                {
                    var validationResponse = await response.Content.ReadFromJsonAsync<TokenValidationResponse>();
                    if (validationResponse.IsValid)
                    {
                        ViewBag.Username = validationResponse.Username;
                        ViewBag.Modules = validationResponse.Modules;
                        return View();
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                // Log the exception or handle it as needed
                // For example: _logger.LogError(ex, "HTTP request error during token validation.");

                // Optionally, add a user-friendly message to TempData and redirect
                TempData["ErrorMessage"] = "There was an error processing your request.";
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                // For example: _logger.LogError(ex, "Unexpected error during token validation.");

                // Optionally, add a user-friendly message to TempData and redirect
                TempData["ErrorMessage"] = "An unexpected error occurred.";
            }

            return RedirectToAction("Index", "Home");
        }

    }
}
