using ManejoTareas.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ManejoTareas.Controllers {
    public class UsuariosController : Controller {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public UsuariosController(UserManager<IdentityUser> userManager, 
                                  SignInManager<IdentityUser> signInManager) {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Registro() { 
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Registro(RegistroViewModel model) {
            if (!ModelState.IsValid) { 
                return View(model); 
            }

            var usuario = new IdentityUser() { 
                Email = model.Email,
                UserName = model.Email
            };

            var resultado = await userManager.CreateAsync(usuario, password: model.Password);

            if (resultado.Succeeded) {
                await signInManager.SignInAsync(usuario, isPersistent: true);

                return RedirectToAction("Index", "Home");
            } else {
                foreach (var error in resultado.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }
        }

        [AllowAnonymous]
        public IActionResult Login() {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }

            var resultado = await signInManager.PasswordSignInAsync(model.Email, 
                                                                    model.Password, 
                                                                    model.Recordar, 
                                                                    lockoutOnFailure: false);

            if (resultado.Succeeded) {
                return RedirectToAction("Index", "Home");
            } else {
                ModelState.AddModelError(string.Empty, "Nombre de usuario o contraseña incorrectos.");

                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout() { 
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}
