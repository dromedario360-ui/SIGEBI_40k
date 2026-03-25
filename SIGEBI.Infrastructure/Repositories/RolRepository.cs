using Npgsql;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Interfaces;
using SIGEBI.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RolRepository : IRepository<Rol>
    {
        private readonly ConexionDb _db;

        public RolRepository(ConexionDb db) => _db = db;

        public async Task<Rol?> ObtenerPorIdAsync(int id)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT * FROM \"Rol\" WHERE \"IdRol\" = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync()) return Mapear(reader);
            return null;
        }

        public async Task<IEnumerable<Rol>> ObtenerTodosAsync()
        {
            var lista = new List<Rol>();
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT * FROM \"Rol\"", conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync()) lista.Add(Mapear(reader));
            return lista;
        }

        public async Task AgregarAsync(Rol entity)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(@"
                INSERT INTO ""Rol"" (""Nombre"", ""Descripcion"", ""Activo"")
                VALUES (@nombre, @descripcion, @activo)", conn);
            cmd.Parameters.AddWithValue("nombre", entity.Nombre);
            cmd.Parameters.AddWithValue("descripcion", (object?)entity.Descripcion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("activo", entity.Activo);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task ActualizarAsync(Rol entity)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(@"
                UPDATE ""Rol"" SET
                ""Nombre"" = @nombre, ""Descripcion"" = @descripcion, ""Activo"" = @activo
                WHERE ""IdRol"" = @id", conn);
            cmd.Parameters.AddWithValue("nombre", entity.Nombre);
            cmd.Parameters.AddWithValue("descripcion", (object?)entity.Descripcion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("activo", entity.Activo);
            cmd.Parameters.AddWithValue("id", entity.Id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EliminarAsync(int id)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "DELETE FROM \"Rol\" WHERE \"IdRol\" = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        private static Rol Mapear(NpgsqlDataReader r)
            => Rol.Crear(
                r["Nombre"].ToString()!,
                r["Descripcion"] == DBNull.Value ? null : r["Descripcion"].ToString());
    }
}
