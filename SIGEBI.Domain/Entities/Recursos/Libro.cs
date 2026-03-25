using SIGEBI.Domain.Base;
using SIGEBI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Entities.Recursos
{
    public class Libro : RecursoBase
    {
        public string? Editorial { get; private set; }
        public int? NumeroPaginas { get; private set; }

        private Libro() { }

        public static Libro Crear(int idCategoria, string codigo, string titulo,
                                   string autor, int stock, string? isbn = null,
                                   string? editorial = null, int? numeroPaginas = null)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new DomainException("El código es obligatorio.");
            if (string.IsNullOrWhiteSpace(titulo))
                throw new DomainException("El título es obligatorio.");
            if (string.IsNullOrWhiteSpace(autor))
                throw new DomainException("El autor es obligatorio.");
            if (stock < 0)
                throw new DomainException("El stock no puede ser negativo.");

            return new Libro
            {
                IdCategoria = idCategoria,
                Codigo = codigo.Trim().ToUpperInvariant(),
                Titulo = titulo.Trim(),
                Autor = autor.Trim(),
                ISBN = isbn?.Trim(),
                Stock = stock,
                Disponible = stock > 0,
                Tipo = TipoRecurso.Libro,
                FechaRegistro = DateTime.Now,
                Editorial = editorial?.Trim(),
                NumeroPaginas = numeroPaginas
            };
        }
    }
}
