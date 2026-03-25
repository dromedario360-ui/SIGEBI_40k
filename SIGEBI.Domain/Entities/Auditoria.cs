using SIGEBI.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Entities
{
    public class Auditoria : BaseEntity
    {
        public int? IdUsuario { get; private set; }
        public string Accion { get; private set; } = null!;
        public string? Descripcion { get; private set; }
        public DateTime Fecha { get; private set; }

        private Auditoria() { }

        public static Auditoria Crear(int? idUsuario, string accion, string? descripcion)
        {
            if (string.IsNullOrWhiteSpace(accion))
                throw new DomainException("La acción de auditoría es obligatoria.");

            return new Auditoria
            {
                IdUsuario = idUsuario,
                Accion = accion.Trim(),
                Descripcion = descripcion?.Trim(),
                Fecha = DateTime.Now
            };
        }
    }
}
