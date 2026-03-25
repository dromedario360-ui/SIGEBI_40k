using SIGEBI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Interfaces
{
    public interface IAuditoriaRepository : IRepository<Auditoria>
    {
        Task<IEnumerable<Auditoria>> ObtenerPorUsuarioAsync(int idUsuario);
        Task<IEnumerable<Auditoria>> ObtenerPorFechaAsync(DateTime desde, DateTime hasta);
    }
}
