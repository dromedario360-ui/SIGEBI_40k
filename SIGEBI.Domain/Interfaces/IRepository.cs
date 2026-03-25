using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<T>> ObtenerTodosAsync();
        Task AgregarAsync(T entity);
        Task ActualizarAsync(T entity);
        Task EliminarAsync(int id);
    }
}
