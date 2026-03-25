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
    public class CategoriaRepository : IRepository<Categoria>
    {
        private readonly ConexionDb _db;

        public CategoriaRepository(ConexionDb db) => _db = db;

        public async Task<Categoria?> ObtenerPorIdAsync(int id)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT * FROM \"Categoria\" WHERE \"IdCategoria\" = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync()) return Mapear(reader);
            return null;
        }

        public async Task<IEnumerable<Categoria>> ObtenerTodosAsync()
        {
            var lista = new List<Categoria>();
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT * FROM \"Categoria\"", conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync()) lista.Add(Mapear(reader));
            return lista;
        }

        public async Task AgregarAsync(Categoria entity)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(@"
                INSERT INTO ""Categoria"" (""Nombre"", ""Descripcion"", ""Activa"")
                VALUES (@nombre, @descripcion, @activa)", conn);
            cmd.Parameters.AddWithValue("nombre", entity.Nombre);
            cmd.Parameters.AddWithValue("descripcion", (object?)entity.Descripcion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("activa", entity.Activa);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task ActualizarAsync(Categoria entity)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(@"
                UPDATE ""Categoria"" SET
                ""Nombre"" = @nombre, ""Descripcion"" = @descripcion, ""Activa"" = @activa
                WHERE ""IdCategoria"" = @id", conn);
            cmd.Parameters.AddWithValue("nombre", entity.Nombre);
            cmd.Parameters.AddWithValue("descripcion", (object?)entity.Descripcion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("activa", entity.Activa);
            cmd.Parameters.AddWithValue("id", entity.Id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EliminarAsync(int id)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "DELETE FROM \"Categoria\" WHERE \"IdCategoria\" = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        private static Categoria Mapear(NpgsqlDataReader r)
            => Categoria.Crear(
                r["Nombre"].ToString()!,
                r["Descripcion"] == DBNull.Value ? null : r["Descripcion"].ToString());
    }
}
