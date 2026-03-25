using SIGEBI.Domain.Base;
using SIGEBI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Entities.Recursos
{
    public class Revista : RecursoBase
    {
        public int? NumeroEdicion { get; private set; }
        public string? Periodicidad { get; private set; }

        private Revista() { }

        public static Revista Crear(int idCategoria, string codigo, string titulo,
                                     string autor, int stock, string? isbn = null,
                                     int? numeroEdicion = null, string? periodicidad = null)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new DomainException("El código es obligatorio.");
            if (string.IsNullOrWhiteSpace(titulo))
                throw new DomainException("El título es obligatorio.");
            if (string.IsNullOrWhiteSpace(autor))
                throw new DomainException("El autor es obligatorio.");
            if (stock < 0)
                throw new DomainException("El stock no puede ser negativo.");

            return new Revista
            {
                IdCategoria = idCategoria,
                Codigo = codigo.Trim().ToUpperInvariant(),
                Titulo = titulo.Trim(),
                Autor = autor.Trim(),
                ISBN = isbn?.Trim(),
                Stock = stock,
                Disponible = stock > 0,
                Tipo = TipoRecurso.Revista,
                FechaRegistro = DateTime.Now,
                NumeroEdicion = numeroEdicion,
                Periodicidad = periodicidad?.Trim()
            };
        }
    }
}
