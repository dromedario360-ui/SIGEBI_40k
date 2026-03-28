using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Infrastructure.Services
{
    public class NotificacionEmailService : INotificacionService
    {
        public async Task NotificarPrestamoCreado(Usuario usuario, Prestamo prestamo)
        {
            Console.WriteLine($"[EMAIL] {usuario.Email} — Préstamo #{prestamo.Id} creado. Fecha límite: {prestamo.FechaLimite:dd/MM/yyyy}");
            await Task.CompletedTask;
        }

        public async Task NotificarDevolucionConMulta(Usuario usuario, Prestamo prestamo, decimal monto)
        {
            Console.WriteLine($"[EMAIL] {usuario.Email} — Devolución procesada. Multa: RD${monto}");
            await Task.CompletedTask;
        }

        public async Task NotificarPrestamoVencido(Usuario usuario, Prestamo prestamo)
        {
            Console.WriteLine($"[EMAIL] {usuario.Email} — Préstamo #{prestamo.Id} está vencido.");
            await Task.CompletedTask;
        }
    }
}
