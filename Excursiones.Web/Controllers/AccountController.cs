using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Excursiones.Data.Repositorios;
using System.Security.Claims;

namespace Excursiones.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public AccountController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository; 
        }
        [HttpGet]
        public IActionResult ExternalLogin(string returnUrl = "/")
        {
           
            var redirectUrl = Url.Action("GoogleResponse", "Account", new { returnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public async Task<IActionResult> GoogleResponse(string returnUrl = "/")
        {
            //  Leemos el resultado del intento de login que hizo Google
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                TempData["Error"] = "No se pudo autenticar con Google.";
                return RedirectToAction("Index", "Home");
            }

            // Extraemos el Email y el Nombre que nos mandó Google en los Claims externos
            var email = result.Principal.FindFirstValue(ClaimTypes.Email);
            var nombre = result.Principal.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "Google no proporcionó un correo electrónico válido.";
                return RedirectToAction("Index", "Home");
            }

           
            var usuarioDb = _usuarioRepository.ObtenerOCrearUsuario(email, nombre ?? "Sin Nombre");
            var accionesPermitidas = _usuarioRepository.ObtenerAcciones(email);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuarioDb.Nombre),
                new Claim(ClaimTypes.Email, usuarioDb.Email),
                new Claim("IdUsuario", usuarioDb.IdPersona.ToString()) 
            };

            // Agregamos cada permiso como un claim de tipo "Accion"
            foreach (var accion in accionesPermitidas)
            {
                claims.Add(new Claim("Accion", accion));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true 
            };

           
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await Microsoft.AspNetCore.Authentication.AuthenticationHttpContextExtensions.SignOutAsync(
                HttpContext,
                Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

    }

}