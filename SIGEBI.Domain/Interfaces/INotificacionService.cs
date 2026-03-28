using SIGEBI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Interfaces
{
    public interface INotificacionService
    {
        Task NotificarPrestamoCreado(Usuario usuario, Prestamo prestamo);
        Task NotificarDevolucionConMulta(Usuario usuario, Prestamo prestamo, decimal monto);
        Task NotificarPrestamoVencido(Usuario usuario, Prestamo prestamo);
    }
}
