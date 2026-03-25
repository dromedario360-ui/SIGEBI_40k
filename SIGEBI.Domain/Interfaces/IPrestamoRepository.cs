using SIGEBI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Interfaces
{
    public interface IPrestamoRepository : IRepository<Prestamo>
    {
        Task<IEnumerable<Prestamo>> ObtenerActivosPorUsuarioAsync(int idUsuario);
        Task<IEnumerable<Prestamo>> ObtenerVencidosAsync();
        Task<Prestamo?> ObtenerConDetallesAsync(int id);
    }
}
