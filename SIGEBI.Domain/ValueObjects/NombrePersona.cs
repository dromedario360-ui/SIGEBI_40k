using SIGEBI.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.ValueObjects
{
    public class NombrePersona : ValueObject
    {
        public string Nombre { get; }
        public string Apellido { get; }
        public string NombreCompleto => $"{Nombre} {Apellido}".Trim();

        private NombrePersona() { Nombre = string.Empty; Apellido = string.Empty; }

        public NombrePersona(string nombre, string apellido)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new DomainException("El nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(apellido))
                throw new DomainException("El apellido es obligatorio.");

            Nombre = nombre.Trim();
            Apellido = apellido.Trim();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Nombre;
            yield return Apellido;
        }

        public override string ToString() => NombreCompleto;
    }
}
