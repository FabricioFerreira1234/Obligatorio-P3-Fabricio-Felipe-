using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaNegocio.IRepositorios
{
    public interface IRepositorio<T> where T : class
    {
        public void Add(T entity);
        public void Update(T entity);
        public void Delete(T entity);
        public T FindById(object id);
    }
}
