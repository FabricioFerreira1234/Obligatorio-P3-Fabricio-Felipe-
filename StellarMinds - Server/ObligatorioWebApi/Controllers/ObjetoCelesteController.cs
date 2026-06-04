using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StellarMinds.DTOs.DataTransferObjects.DTOsObjetoCeleste;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUObservacion;
using System.Collections.Generic;

namespace ObligatorioWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // RF10 - Cualquier rol autenticado.
    public class ObjetoCelesteController : ControllerBase
    {
        private readonly ICURankingObjetosCelestes _cuRanking;

        public ObjetoCelesteController(ICURankingObjetosCelestes cuRanking)
        {
            _cuRanking = cuRanking;
        }

        // RF10 - Ranking de objetos celestes observados por los socios: nombre, tipo y cantidad
        // de veces observado, ordenado de mayor a menor. Los nunca observados no aparecen.
        [HttpGet("ranking")]
        public IActionResult Ranking()
        {
            try
            {
                List<DTORankingObjetoCeleste> ranking = _cuRanking.Ejecutar();
                return Ok(ranking);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocurrió un error al obtener el ranking de objetos celestes." });
            }
        }
    }
}
