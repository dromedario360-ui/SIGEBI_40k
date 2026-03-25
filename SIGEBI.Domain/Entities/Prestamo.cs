using SIGEBI.Domain.Base;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Entities
{
    public class Prestamo : BaseEntity
    {
        public int IdUsuario { get; private set; }
        public DateTime FechaPrestamo { get; private set; }
        public DateTime FechaLimite { get; private set; }
        public EstadoPrestamo Estado { get; private set; }
        public Dinero TotalMulta { get; private set; } = null!;

        private readonly List<DetallePrestamo> _detalles = new();
        public IReadOnlyCollection<DetallePrestamo> Detalles => _detalles.AsReadOnly();

        private Prestamo() { }

        public static Prestamo Crear(int idUsuario, DateTime fechaLimite)
        {
            if (idUsuario <= 0)
                throw new DomainException("El usuario es inválido.");
            if (fechaLimite.Date <= DateTime.Now.Date)
                throw new DomainException("La fecha límite debe ser una fecha futura.");

            return new Prestamo
            {
                IdUsuario = idUsuario,
                FechaPrestamo = DateTime.Now,
                FechaLimite = fechaLimite,
                Estado = EstadoPrestamo.ACTIVO,
                TotalMulta = new Dinero(0)
            };
        }

        public void AgregarDetalle(int idRecurso, int cantidad)
        {
            if (Estado != EstadoPrestamo.ACTIVO)
                throw new DomainException("No se puede modificar un préstamo que no está activo.");

            var detalle = DetallePrestamo.Crear(Id, idRecurso, cantidad);
            _detalles.Add(detalle);
        }

        public void AplicarMulta(Dinero monto)
        {
            if (monto.Monto < 0)
                throw new DomainException("La multa no puede ser negativa.");
            TotalMulta = monto;
        }

        public void MarcarComoDevuelto() => Estado = EstadoPrestamo.DEVUELTO;
        public void MarcarComoVencido() => Estado = EstadoPrestamo.VENCIDO;
    }
}
