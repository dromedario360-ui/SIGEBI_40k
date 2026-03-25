using SIGEBI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Interfaces
{

    public interface IPenalizacionRepository : IRepository<Penalizacion>
    {
        Task<bool> UsuarioTienePenalizacionActivaAsync(int idUsuario);
        Task<IEnumerable<Penalizacion>> ObtenerActivasPorUsuarioAsync(int idUsuario);
    }
}
