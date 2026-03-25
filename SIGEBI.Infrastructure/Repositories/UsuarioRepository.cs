using Npgsql;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Interfaces;
using SIGEBI.Domain.ValueObjects;
using SIGEBI.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ConexionDb _db;

        public UsuarioRepository(ConexionDb db) => _db = db;

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT * FROM \"Usuario\" WHERE \"IdUsuario\" = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync()) return Mapear(reader);
            return null;
        }

        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
        {
            var lista = new List<Usuario>();
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT * FROM \"Usuario\"", conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync()) lista.Add(Mapear(reader));
            return lista;
        }

        public async Task AgregarAsync(Usuario entity)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(@"
                INSERT INTO ""Usuario""
                (""IdRol"", ""Nombre"", ""Email"", ""PasswordHash"", 
                 ""Telefono"", ""Direccion"", ""Activo"", ""FechaRegistro"")
                VALUES (@idRol, @nombre, @email, @hash, 
                        @telefono, @direccion, @activo, @fecha)", conn);
            cmd.Parameters.AddWithValue("idRol", entity.IdRol);
            cmd.Parameters.AddWithValue("nombre", entity.Nombre.NombreCompleto);
            cmd.Parameters.AddWithValue("email", entity.Email.Value);
            cmd.Parameters.AddWithValue("hash", entity.PasswordHash);
            cmd.Parameters.AddWithValue("telefono", (object?)entity.Telefono ?? DBNull.Value);
            cmd.Parameters.AddWithValue("direccion", (object?)entity.Direccion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("activo", entity.Activo);
            cmd.Parameters.AddWithValue("fecha", entity.FechaRegistro);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task ActualizarAsync(Usuario entity)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(@"
                UPDATE ""Usuario"" SET
                ""Nombre"" = @nombre, ""Email"" = @email,
                ""Telefono"" = @telefono, ""Direccion"" = @direccion,
                ""Activo"" = @activo
                WHERE ""IdUsuario"" = @id", conn);
            cmd.Parameters.AddWithValue("nombre", entity.Nombre.NombreCompleto);
            cmd.Parameters.AddWithValue("email", entity.Email.Value);
            cmd.Parameters.AddWithValue("telefono", (object?)entity.Telefono ?? DBNull.Value);
            cmd.Parameters.AddWithValue("direccion", (object?)entity.Direccion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("activo", entity.Activo);
            cmd.Parameters.AddWithValue("id", entity.Id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EliminarAsync(int id)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "DELETE FROM \"Usuario\" WHERE \"IdUsuario\" = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<Usuario?> ObtenerPorEmailAsync(string email)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT * FROM \"Usuario\" WHERE LOWER(\"Email\") = LOWER(@email)", conn);
            cmd.Parameters.AddWithValue("email", email);
            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync()) return Mapear(reader);
            return null;
        }

        public async Task<bool> ExisteEmailAsync(string email)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT COUNT(1) FROM \"Usuario\" WHERE LOWER(\"Email\") = LOWER(@email)", conn);
            cmd.Parameters.AddWithValue("email", email);
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt64(result) > 0;
        }

        private static Usuario Mapear(NpgsqlDataReader r)
        {
            var partes = r["Nombre"].ToString()!.Split(' ', 2);
            var nombre = new NombrePersona(
                partes[0],
                partes.Length > 1 ? partes[1] : ".");
            var email = new Email(r["Email"].ToString()!);
            return Usuario.Crear(
                Convert.ToInt32(r["IdRol"]),
                nombre, email,
                r["PasswordHash"].ToString()!);
        }
    }
}
