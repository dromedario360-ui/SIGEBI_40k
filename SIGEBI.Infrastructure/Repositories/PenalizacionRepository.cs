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
    public class PenalizacionRepository : IPenalizacionRepository
    {
        private readonly ConexionDb _db;

        public PenalizacionRepository(ConexionDb db) => _db = db;

        public async Task<Penalizacion?> ObtenerPorIdAsync(int id)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT * FROM \"Penalizacion\" WHERE \"IdPenalizacion\" = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync()) return Mapear(reader);
            return null;
        }

        public async Task<IEnumerable<Penalizacion>> ObtenerTodosAsync()
        {
            var lista = new List<Penalizacion>();
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT * FROM \"Penalizacion\"", conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync()) lista.Add(Mapear(reader));
            return lista;
        }

        public async Task AgregarAsync(Penalizacion entity)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(@"
                INSERT INTO ""Penalizacion""
                (""IdUsuario"", ""Motivo"", ""Monto"", ""FechaInicio"", ""Activa"")
                VALUES (@idUsuario, @motivo, @monto, @fechaInicio, @activa)", conn);
            cmd.Parameters.AddWithValue("idUsuario", entity.IdUsuario);
            cmd.Parameters.AddWithValue("motivo", entity.Motivo);
            cmd.Parameters.AddWithValue("monto", entity.Monto.Monto);
            cmd.Parameters.AddWithValue("fechaInicio", entity.FechaInicio);
            cmd.Parameters.AddWithValue("activa", entity.Activa);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task ActualizarAsync(Penalizacion entity)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(@"
                UPDATE ""Penalizacion"" SET
                ""Activa"" = @activa, ""FechaFin"" = @fechaFin
                WHERE ""IdPenalizacion"" = @id", conn);
            cmd.Parameters.AddWithValue("activa", entity.Activa);
            cmd.Parameters.AddWithValue("fechaFin", (object?)entity.FechaFin ?? DBNull.Value);
            cmd.Parameters.AddWithValue("id", entity.Id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EliminarAsync(int id)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "DELETE FROM \"Penalizacion\" WHERE \"IdPenalizacion\" = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> UsuarioTienePenalizacionActivaAsync(int idUsuario)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(@"
                SELECT COUNT(1) FROM ""Penalizacion""
                WHERE ""IdUsuario"" = @id AND ""Activa"" = true", conn);
            cmd.Parameters.AddWithValue("id", idUsuario);
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt64(result) > 0;
        }

        public async Task<IEnumerable<Penalizacion>> ObtenerActivasPorUsuarioAsync(int idUsuario)
        {
            var lista = new List<Penalizacion>();
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(@"
                SELECT * FROM ""Penalizacion""
                WHERE ""IdUsuario"" = @id AND ""Activa"" = true", conn);
            cmd.Parameters.AddWithValue("id", idUsuario);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync()) lista.Add(Mapear(reader));
            return lista;
        }

        private static Penalizacion Mapear(NpgsqlDataReader r)
            => Penalizacion.Crear(
                Convert.ToInt32(r["IdUsuario"]),
                r["Motivo"].ToString()!,
                new Dinero(Convert.ToDecimal(r["Monto"])));
    }
}
