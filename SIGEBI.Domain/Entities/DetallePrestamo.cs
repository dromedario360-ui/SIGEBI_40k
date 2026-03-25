using SIGEBI.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Entities
{
    public class DetallePrestamo : BaseEntity
    {
        public int IdPrestamo { get; private set; }
        public int IdRecurso { get; private set; }
        public int Cantidad { get; private set; }

        private DetallePrestamo() { }

        internal static DetallePrestamo Crear(int idPrestamo, int idRecurso, int cantidad)
        {
            if (idRecurso <= 0)
                throw new DomainException("El recurso es inválido.");
            if (cantidad <= 0)
                throw new DomainException("La cantidad debe ser mayor a cero.");

            return new DetallePrestamo
            {
                IdPrestamo = idPrestamo,
                IdRecurso = idRecurso,
                Cantidad = cantidad
            };
        }
    }
}
