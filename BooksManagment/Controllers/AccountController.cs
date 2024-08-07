using BooksManagment.Models;
using BooksManagment.ViewModels;
using BooksManagment.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BooksManagment.Controllers{
    public class AccountController : Controller
    {
        private readonly UserService _userService;

        public AccountController(UserService userService)
        {
            _userService = userService;
        }
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Username,Password")] LoginViewModel model)
        {
            /*if (ModelState.IsValid) {
                var user = await _userService.Authenticate(model.Username, model.Password);
                if (user != null) {
                    // Implementar la lógica de autenticación (cookie, token, etc.)
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Intento de inicio de sesión inválido.");
            }
            return View(model);*/
            if(ModelState.IsValid){
                var user = await _userService.Authenticate(model.Username, model.Password);
                if(user!=null){
                    var claims = new List<Claim> {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.Role?.Name ?? "Invitado")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties {
                        IsPersistent = true,
                        //ExpiresUtc = System.DateTimeOffset.UtcNow.AddDays(7)
                        ExpiresUtc = System.DateTimeOffset.UtcNow.AddHours(1)
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Intento de inicio de sesión inválido.");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var defaultRole = 2; // Roles -> 1:Administrador, 2:Normal, 3:Invitado
                var user = new User { Username = model.Username, Password = model.Password, Email = model.Email, RoleID = defaultRole };

                if (await _userService.UserExists(user.Username)) {
                    ModelState.AddModelError(string.Empty, "El nombre de usuario ya existe");
                    return View(model);
                }
                await _userService.Register(user);
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Implementar la lógica de cierre de sesión (eliminar cookies, etc.)
            return RedirectToAction("Index", "Home");
        }
    }
}