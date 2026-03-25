using SIGEBI.Domain.Base;
using SIGEBI.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Entities
{
    public class Usuario : BaseEntity
    {
        public int IdRol { get; private set; }
        public NombrePersona Nombre { get; private set; } = null!;
        public Email Email { get; private set; } = null!;
        public string PasswordHash { get; private set; } = null!;
        public string? Telefono { get; private set; }
        public string? Direccion { get; private set; }
        public bool Activo { get; private set; }
        public DateTime FechaRegistro { get; private set; }

        private Usuario() { }

        public static Usuario Crear(int idRol, NombrePersona nombre, Email email, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new DomainException("La contraseña es obligatoria.");

            return new Usuario
            {
                IdRol = idRol,
                Nombre = nombre,
                Email = email,
                PasswordHash = passwordHash,
                Activo = true,
                FechaRegistro = DateTime.Now
            };
        }

        public void CambiarEmail(Email nuevoEmail) => Email = nuevoEmail;
        public void ActualizarPerfil(string? telefono, string? direccion)
        {
            Telefono = telefono?.Trim();
            Direccion = direccion?.Trim();
        }
        public void CambiarPassword(string nuevoHash)
        {
            if (string.IsNullOrWhiteSpace(nuevoHash))
                throw new DomainException("La contraseña no puede estar vacía.");
            PasswordHash = nuevoHash;
        }
        public void Activar() => Activo = true;
        public void Desactivar() => Activo = false;
    }
}
