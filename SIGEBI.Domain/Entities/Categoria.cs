using SIGEBI.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Entities
{
    public class Categoria : BaseEntity
    {
        public string Nombre { get; private set; } = null!;
        public string? Descripcion { get; private set; }
        public bool Activa { get; private set; }

        private Categoria() { }

        public static Categoria Crear(string nombre, string? descripcion)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new DomainException("El nombre de la categoría es obligatorio.");

            return new Categoria
            {
                Nombre = nombre.Trim(),
                Descripcion = descripcion?.Trim(),
                Activa = true
            };
        }

        public void Actualizar(string nombre, string? descripcion)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new DomainException("El nombre de la categoría es obligatorio.");
            Nombre = nombre.Trim();
            Descripcion = descripcion?.Trim();
        }

        public void Activar() => Activa = true;
        public void Desactivar() => Activa = false;
    }
}
