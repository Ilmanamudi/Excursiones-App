using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Excursiones.Web.Seguridad
{
    public class TieneAccionAttribute : TypeFilterAttribute
    {
        public TieneAccionAttribute(string accionRequerida) : base(typeof(AccionAuthorizeFilter))
        {
            Arguments = new object[] { accionRequerida };
        }
    }

    public class AccionAuthorizeFilter : IAuthorizationFilter
    {
        private readonly string _accionRequerida;

        public AccionAuthorizeFilter(string accionRequerida)
        {
            _accionRequerida = accionRequerida;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Verificamos si el usuario está autenticado
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new ChallengeResult();
                return;
            }

            // Buscamos si la cookie contiene un claim del tipo "Accion" que coincida con la requerida
            var tienePermiso = context.HttpContext.User.Claims
                .Any(c => c.Type == "Accion" && c.Value == _accionRequerida);

            if (!tienePermiso)
            {
                // Si no tiene el permiso, lo redirigimos a una vista de acceso denegado o al inicio
                context.Result = new RedirectToActionResult("AccesoDenegado", "Home", null);
            }
        }
    }
}
