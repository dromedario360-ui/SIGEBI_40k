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
    public class AuditoriaRepository : IAuditoriaRepository
    {
        private readonly ConexionDb _db;

        public AuditoriaRepository(ConexionDb db) => _db = db;

        public async Task<Auditoria?> ObtenerPorIdAsync(int id)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT * FROM \"Auditoria\" WHERE \"IdAuditoria\" = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync()) return Mapear(reader);
            return null;
        }

        public async Task<IEnumerable<Auditoria>> ObtenerTodosAsync()
        {
            var lista = new List<Auditoria>();
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT * FROM \"Auditoria\" ORDER BY \"Fecha\" DESC", conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync()) lista.Add(Mapear(reader));
            return lista;
        }

        public async Task AgregarAsync(Auditoria entity)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(@"
                INSERT INTO ""Auditoria""
                (""IdUsuario"", ""Accion"", ""Descripcion"", ""Fecha"")
                VALUES (@idUsuario, @accion, @descripcion, @fecha)", conn);
            cmd.Parameters.AddWithValue("idUsuario", (object?)entity.IdUsuario ?? DBNull.Value);
            cmd.Parameters.AddWithValue("accion", entity.Accion);
            cmd.Parameters.AddWithValue("descripcion", (object?)entity.Descripcion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("fecha", entity.Fecha);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task ActualizarAsync(Auditoria entity) => await Task.CompletedTask;
        public async Task EliminarAsync(int id) => await Task.CompletedTask;

        public async Task<IEnumerable<Auditoria>> ObtenerPorUsuarioAsync(int idUsuario)
        {
            var lista = new List<Auditoria>();
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(@"
                SELECT * FROM ""Auditoria""
                WHERE ""IdUsuario"" = @id
                ORDER BY ""Fecha"" DESC", conn);
            cmd.Parameters.AddWithValue("id", idUsuario);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync()) lista.Add(Mapear(reader));
            return lista;
        }

        public async Task<IEnumerable<Auditoria>> ObtenerPorFechaAsync(DateTime desde, DateTime hasta)
        {
            var lista = new List<Auditoria>();
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(@"
                SELECT * FROM ""Auditoria""
                WHERE ""Fecha"" BETWEEN @desde AND @hasta
                ORDER BY ""Fecha"" DESC", conn);
            cmd.Parameters.AddWithValue("desde", desde);
            cmd.Parameters.AddWithValue("hasta", hasta);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync()) lista.Add(Mapear(reader));
            return lista;
        }

        private static Auditoria Mapear(NpgsqlDataReader r)
            => Auditoria.Crear(
                r["IdUsuario"] == DBNull.Value ? null : Convert.ToInt32(r["IdUsuario"]),
                r["Accion"].ToString()!,
                r["Descripcion"] == DBNull.Value ? null : r["Descripcion"].ToString());
    }
}
