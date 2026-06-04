using StellarMinds.DTOs.DataTransferObjects.DTOsObservacion;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUObservacion;
using StellarMinds.LogicaAplicacion.Servicios;
using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.Enumeraciones;
using StellarMinds.LogicaNegocio.Excepciones;
using StellarMinds.LogicaNegocio.IRepositorios;

namespace StellarMinds.LogicaAplicacion.CasosUso.CUObservacion
{
    // RF07 - Valida las reglas de la observación y evalúa la adecuación del equipo SIN guardar nada.
    public class CUEvaluarAdecuacion : ICUEvaluarAdecuacion
    {
        private readonly IRepositorioPrestamo _repoPrestamo;
        private readonly IRepositorioUsuario _repoUsuario;
        private readonly IRepositorioObjetoCeleste _repoObjeto;
        private readonly IServicioEvaluacionAdecuacion _servicioEvaluacion;

        public CUEvaluarAdecuacion(IRepositorioPrestamo repoPrestamo, IRepositorioUsuario repoUsuario,
            IRepositorioObjetoCeleste repoObjeto, IServicioEvaluacionAdecuacion servicioEvaluacion)
        {
            _repoPrestamo = repoPrestamo;
            _repoUsuario = repoUsuario;
            _repoObjeto = repoObjeto;
            _servicioEvaluacion = servicioEvaluacion;
        }

        public DTOResultadoEvaluacion Ejecutar(string email, DTOAltaObservacion dto)
        {
            if (dto.PrestamoId <= 0)
                throw new ObservacionException("Debe seleccionar el préstamo a observar.");

            if (dto.ObjetoCelesteId <= 0)
                throw new ObservacionException("Debe seleccionar el objeto celeste a observar.");

            Usuario socio = _repoUsuario.FindByEmail(email);
            if (socio == null)
                throw new ObservacionException("No se encontró el socio logueado.");

            Prestamo prestamo = _repoPrestamo.ObtenerPorId(dto.PrestamoId);
            if (prestamo == null)
                throw new ObservacionException("El préstamo seleccionado no existe.");

            if (prestamo.UsuarioId != socio.Id)
                throw new ObservacionException("El préstamo seleccionado no pertenece al socio logueado.");

            if (prestamo.Estado != EstadoPrestamo.EN_PRESTAMO)
                throw new ObservacionException("El préstamo del equipo debe estar EN PRÉSTAMO.");

            if (dto.Fecha.Date < prestamo.Fecha.Inicio.Date || dto.Fecha.Date > prestamo.Fecha.Fin.Date)
                throw new ObservacionException("En la fecha de observación el préstamo del equipo no está vigente.");

            ObjetoCeleste objeto = _repoObjeto.ObtenerPorId(dto.ObjetoCelesteId);
            if (objeto == null)
                throw new ObservacionException("El objeto celeste seleccionado no existe.");

            return _servicioEvaluacion.Evaluar(prestamo, objeto);
        }
    }
}
