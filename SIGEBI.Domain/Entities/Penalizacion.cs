using SIGEBI.Domain.Base;
using SIGEBI.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Entities
{
    public class Penalizacion : BaseEntity
    {
        public int IdUsuario { get; private set; }
        public string Motivo { get; private set; } = null!;
        public Dinero Monto { get; private set; } = null!;
        public DateTime FechaInicio { get; private set; }
        public DateTime? FechaFin { get; private set; }
        public bool Activa { get; private set; }

        private Penalizacion() { }

        public static Penalizacion Crear(int idUsuario, string motivo, Dinero monto)
        {
            if (idUsuario <= 0)
                throw new DomainException("El usuario es inválido.");
            if (string.IsNullOrWhiteSpace(motivo))
                throw new DomainException("El motivo es obligatorio.");

            return new Penalizacion
            {
                IdUsuario = idUsuario,
                Motivo = motivo.Trim(),
                Monto = monto,
                FechaInicio = DateTime.Now,
                Activa = true
            };
        }

        public void Finalizar()
        {
            if (!Activa)
                throw new DomainException("La penalización ya está finalizada.");
            Activa = false;
            FechaFin = DateTime.Now;
        }
    }
}
