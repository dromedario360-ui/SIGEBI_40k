using SIGEBI.Domain.Base;
using SIGEBI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Entities.Recursos
{
    public abstract class RecursoBase : BaseEntity
    {
        public int IdCategoria { get; protected set; }
        public string Codigo { get; protected set; } = null!;
        public string Titulo { get; protected set; } = null!;
        public string Autor { get; protected set; } = null!;
        public string? ISBN { get; protected set; }
        public int Stock { get; protected set; }
        public bool Disponible { get; protected set; }
        public TipoRecurso Tipo { get; protected set; }
        public DateTime FechaRegistro { get; protected set; }

        protected RecursoBase() { }

        public void ReducirStock(int cantidad)
        {
            if (cantidad <= 0)
                throw new DomainException("La cantidad debe ser mayor a cero.");
            if (cantidad > Stock)
                throw new DomainException($"Stock insuficiente. Disponible: {Stock}, solicitado: {cantidad}.");
            Stock -= cantidad;
            Disponible = Stock > 0;
        }

        public void AumentarStock(int cantidad)
        {
            if (cantidad <= 0)
                throw new DomainException("La cantidad debe ser mayor a cero.");
            Stock += cantidad;
            Disponible = true;
        }

        public void Actualizar(string titulo, string autor, int idCategoria, string? isbn)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new DomainException("El título es obligatorio.");
            if (string.IsNullOrWhiteSpace(autor))
                throw new DomainException("El autor es obligatorio.");
            Titulo = titulo.Trim();
            Autor = autor.Trim();
            IdCategoria = idCategoria;
            ISBN = isbn?.Trim();
        }
    }
}
