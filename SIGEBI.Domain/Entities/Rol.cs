using SIGEBI.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Entities
{
    public class Rol : BaseEntity
    {
        public string Nombre { get; private set; }
        public string? Descripcion { get; private set; }
        public bool Activo { get; private set; }

        private Rol() { Nombre = string.Empty; }

        public static Rol Crear(string nombre, string? descripcion)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new DomainException("El nombre del rol es obligatorio.");

            return new Rol
            {
                Nombre = nombre.Trim(),
                Descripcion = descripcion?.Trim(),
                Activo = true
            };
        }

        public void Activar() => Activo = true;
        public void Desactivar() => Activo = false;
        public void ActualizarDescripcion(string? descripcion)
            => Descripcion = descripcion?.Trim();
    }
}
