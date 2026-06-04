using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StellarMinds.WebApp.Auxiliar;
using StellarMinds.WebApp.Enums;
using StellarMinds.WebApp.Filter;
using StellarMinds.WebApp.Models;

namespace StellarMinds.WebApp.Controllers
{
    // RF10 - Ranking de objetos celestes observados (cualquier rol). Consume la WebAPI vía ClienteHttpAuxiliar.
    [LoginFilter]
    public class RankingController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var modelo = new RankingObjetosViewModel();

            string token = HttpContext.Session.GetString("token");
            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                ClienteHttpAuxiliar.UrlBase + "ObjetoCeleste/ranking", VerbosHttp.GET, null, token);

            if (respuesta.IsSuccessStatusCode)
            {
                modelo.Ranking = JsonConvert.DeserializeObject<List<RankingObjetoCelesteModel>>(
                    ClienteHttpAuxiliar.ObtenerBody(respuesta)) ?? new();
            }
            else
            {
                ViewBag.Error = RespuestaApi.LeerError(respuesta);
            }

            return View(modelo);
        }
    }
}
