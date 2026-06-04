using StellarMinds.LogicaNegocio.Entidades;
using System.Collections.Generic;

namespace StellarMinds.LogicaNegocio.IRepositorios
{
    public interface IRepositorioAuditoria
    {
        void Add(Auditoria a);
        List<Auditoria> FindAll();
    }
}
