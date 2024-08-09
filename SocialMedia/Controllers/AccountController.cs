using SocialMedia.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace SocialMedia.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model) {
            if (ModelState.IsValid) {
                // Crear el usuario y guardarlo en la base de datos
                var user = new ApplicationUser { 
                    UserName = model.Email, 
                    Email = model.Email,
                    Name = model.Name,
                    LastName = model.LastName
                };
                // Validar que el nombre de usuario y el correo no estén en uso
                var result = await _userManager.CreateAsync(user, model.Password);

                // Si el usuario fue creado correctamente, iniciar sesión y redireccionar al home
                if (result.Succeeded) {
                    // Se ha iniciado sesión para el usuario
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                // Si hubo algún error al crear el usuario, mostrar los errores en el formulario
                foreach (var error in result.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string? ReturnUrl = null)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string? ReturnUrl = null){
            if (ModelState.IsValid) {
                // Verificar si el usuario y la contraseña son válidos
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                // Si el usuario y la contraseña son válidos, redireccionar al home
                if (result.Succeeded) {
                    if(!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl)){
                        return Redirect(ReturnUrl);
                    }else{
                        return RedirectToAction("Index", "Home");
                    }
                }
                if(result.RequiresTwoFactor) {
                    // Instrucciones para activar el two-factor authentication (SMS, email, etc.)
                }
                if(result.IsLockedOut) {
                    // Instrucciones para desbloquear la cuenta
                }
                else {
                    ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout() {
            // Cerrar sesión del usuario
            await _signInManager.SignOutAsync();
            // Redirigir a la página de inicio
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied(){
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [HttpGet]
        public  async Task<IActionResult> IsEmailAvailable(string email){
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null) {return Json(true); }
            else { return Json($"El correo electrónico {email} ya está en uso"); }
        }        
    }
}