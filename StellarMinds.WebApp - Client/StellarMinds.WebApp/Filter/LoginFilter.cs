using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StellarMinds.WebApp.Filter
{
    // Filtro de seguridad: si no hay token en sesión, redirige al login (igual al ejemplo de cátedra).
    public class LoginFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string token = context.HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
            }
        }
    }
}
