using ManejoTareas.Models;
using ManejoTareas.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ManejoTareas.Controllers {
    public class UsuariosController : Controller {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly ApplicationDbContext context;

        public UsuariosController(UserManager<IdentityUser> userManager, 
                                  SignInManager<IdentityUser> signInManager,
                                  ApplicationDbContext context) {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
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
        public IActionResult Login(string msg = null) {
            if (msg is not null) { 
                ViewData["Mensaje"] = msg;  
            }

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

        [AllowAnonymous]
        public async Task<IActionResult> RegistrarUsuarioExterno(string urlRetorno = null, string remoteError = null) {
            urlRetorno = urlRetorno ?? Url.Content("~/");
            var msg = "";

            if (remoteError is not null) {
                msg = $"Error del proveedor externo { remoteError }";

                return RedirectToAction("Login", routeValues: new { msg });
            }

            var info = await signInManager.GetExternalLoginInfoAsync();

            if (info is null) {
                msg = $"Error, cargando la data del login externo";

                return RedirectToAction("Login", routeValues: new { msg });
            }

            var resultado = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, 
                                                                         info.ProviderKey, 
                                                                         isPersistent: true, 
                                                                         bypassTwoFactor: true);

            /* La cuenta existe */
            if (resultado.Succeeded) { 
                return LocalRedirect(urlRetorno);
            }

            string email = "";

            /* Sino esta registrado el usuario, lo registramos */
            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email)) { 
                email = info.Principal.FindFirstValue(ClaimTypes.Email);
            } else {
                msg = "Error, leyendo el email del usuario del proveedor";

                return RedirectToAction("Login", routeValues: new { msg });
            }

            var usuario = new IdentityUser { Email = email, UserName = email };
            var crearUsuario = await userManager.CreateAsync(usuario);

            if (!crearUsuario.Succeeded) {
                msg = crearUsuario.Errors.First().Description;

                return RedirectToAction("Login", routeValues: new { msg });
            }

            var agregarUsuario = await userManager.AddLoginAsync(usuario, info);

            if (agregarUsuario.Succeeded) { 
                await signInManager.SignInAsync(usuario, isPersistent: true, info.LoginProvider);

                return LocalRedirect(urlRetorno);
            }

            msg = "Ha ocurrido un error agregando al login";
            return RedirectToAction("Login", routeValues: new { msg });
        }   

        [AllowAnonymous]
        [HttpGet]
        public ChallengeResult LoginExterno(string proveedor, string urlRetorno) {
            var urlRedireccion = Url.Action("RegistrarUsuarioExterno", values: new { urlRetorno });
            var propiedades = signInManager.ConfigureExternalAuthenticationProperties(proveedor, urlRedireccion);

            return new ChallengeResult(proveedor, propiedades);
        }

        [Authorize(Roles = Constantes.RolAdmin)]
        [HttpGet]
        public async Task<IActionResult> Listado(string msg = null) {
            var usuarios = await context.Users.Select(u => new UsuarioViewModel { 
                                                    Email = u.Email
                                               }).ToListAsync();

            var modelo = new UsuariosListadoViewModel();
            modelo.Usuarios = usuarios;
            modelo.Mensaje = msg;

            return View(modelo);
        }

        [Authorize(Roles = Constantes.RolAdmin)]
        [HttpPost]
        public async Task<IActionResult> HacerAdmin(string email) { 
            var usuario = await context.Users.Where(u => u.Email == email)
                                             .FirstOrDefaultAsync();

            if (usuario is null) {
                return NotFound();
            }

            await userManager.AddToRoleAsync(usuario, Constantes.RolAdmin);

            return RedirectToAction("Listado", routeValues: new { msg = $"Rol asignado correctamente a { email }" });
        }

        [Authorize(Roles = Constantes.RolAdmin)]
        [HttpPost]
        public async Task<IActionResult> RemoverAdmin(string email) {
            var usuario = await context.Users.Where(u => u.Email == email)
                                             .FirstOrDefaultAsync();

            if (usuario is null)
            {
                return NotFound();
            }

            await userManager.RemoveFromRoleAsync(usuario, Constantes.RolAdmin);

            return RedirectToAction("Listado", routeValues: new { msg = $"Rol removido correctamente a { email }" });
        }
    }
}
