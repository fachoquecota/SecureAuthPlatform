using AuthUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using System.Text;
using AuthUI.Models.Response;

namespace AuthUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }

            try
            {
                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    var content = new StringContent(JsonSerializer.Serialize(loginModel), Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync("http://localhost:5101/Auth/login", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        var result = JsonSerializer.Deserialize<JwtTokenResponse>(responseString);
                        if (result != null && !string.IsNullOrEmpty(result.token))
                        {
                            var cookieOptions = new CookieOptions
                            {
                                HttpOnly = true,
                                Secure = true,
                                SameSite = SameSiteMode.Strict
                            };
                            Response.Cookies.Append("jwt", result.token, cookieOptions);

                            return RedirectToAction("Index", "Users");
                        }
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        TempData["ErrorMessage"] = "Invalid username or password.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "There was an error processing your request.";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login process");
                TempData["ErrorMessage"] = "An unexpected error occurred.";
            }

            return View(loginModel);
        }
    }
}
