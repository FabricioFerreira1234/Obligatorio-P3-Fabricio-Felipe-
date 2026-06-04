using StellarMinds.DTOs.DataTransferObjects.DTOsObservacion;
using StellarMinds.DTOs.Mappers;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUObservacion;
using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.IRepositorios;

namespace StellarMinds.LogicaAplicacion.CasosUso.CUObservacion
{
    // RF07 - Confirma el alta: reutiliza la evaluación (que valida todas las reglas) y guarda
    // la observación junto con el resultado IDEAL/ADECUADO/NO RECOMENDABLE y su motivo.
    public class CUAltaObservacion : ICUAltaObservacion
    {
        private readonly ICUEvaluarAdecuacion _cuEvaluar;
        private readonly IRepositorioObservacion _repoObservacion;

        public CUAltaObservacion(ICUEvaluarAdecuacion cuEvaluar, IRepositorioObservacion repoObservacion)
        {
            _cuEvaluar = cuEvaluar;
            _repoObservacion = repoObservacion;
        }

        public void Ejecutar(string email, DTOAltaObservacion dto)
        {
            DTOResultadoEvaluacion evaluacion = _cuEvaluar.Ejecutar(email, dto);

            Observacion observacion = MapperObservacion.ToObservacion(dto);
            observacion.ResultadoIA = evaluacion.Indicador;
            observacion.MotivoIA = evaluacion.Detalle;
            observacion.Validar();

            _repoObservacion.Add(observacion);
        }
    }
}
