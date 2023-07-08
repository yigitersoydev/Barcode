using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BarcodeGenerator.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace BarcodeGenerator.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Login(User user) {
            var username = _configuration.GetValue<string>("Credential:Username");
            var password = _configuration.GetValue<string>("Credential:Password");

            if(username == user.Username && password == user.Password) {
                var claims = new List<Claim>
                {
                     new Claim(ClaimTypes.Name,user.Username)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties();

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            }
            return RedirectToAction("CreateBarcode", "Home");
        }
    }
}

