using SIGEBI.Domain.Entities.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Interfaces
{
    public interface IRecursoRepository : IRepository<RecursoBase>
    {
        Task<RecursoBase?> ObtenerPorCodigoAsync(string codigo);
        Task<IEnumerable<RecursoBase>> ObtenerDisponiblesAsync();
        Task<IEnumerable<RecursoBase>> ObtenerPorCategoriaAsync(int idCategoria);
    }
}
