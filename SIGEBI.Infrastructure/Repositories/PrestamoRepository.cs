using Npgsql;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
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
    public class PrestamoRepository : IPrestamoRepository
    {
        private readonly ConexionDb _db;

        public PrestamoRepository(ConexionDb db) => _db = db;

        public async Task<Prestamo?> ObtenerPorIdAsync(int id)
            => await ObtenerConDetallesAsync(id);

        public async Task<IEnumerable<Prestamo>> ObtenerTodosAsync()
        {
            var lista = new List<Prestamo>();
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT * FROM \"Prestamo\"", conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync()) lista.Add(MapearPrestamo(reader));
            return lista;
        }

        public async Task AgregarAsync(Prestamo entity)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(@"
                INSERT INTO ""Prestamo""
                (""IdUsuario"", ""FechaPrestamo"", ""FechaLimite"", ""Estado"", ""TotalMulta"")
                VALUES (@idUsuario, @fechaPrestamo, @fechaLimite, @estado, @multa)
                RETURNING ""IdPrestamo""", conn);
            cmd.Parameters.AddWithValue("idUsuario", entity.IdUsuario);
            cmd.Parameters.AddWithValue("fechaPrestamo", entity.FechaPrestamo);
            cmd.Parameters.AddWithValue("fechaLimite", entity.FechaLimite);
            cmd.Parameters.AddWithValue("estado", entity.Estado.ToString());
            cmd.Parameters.AddWithValue("multa", entity.TotalMulta.Monto);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task ActualizarAsync(Prestamo entity)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(@"
                UPDATE ""Prestamo"" SET
                ""Estado"" = @estado, ""TotalMulta"" = @multa
                WHERE ""IdPrestamo"" = @id", conn);
            cmd.Parameters.AddWithValue("estado", entity.Estado.ToString());
            cmd.Parameters.AddWithValue("multa", entity.TotalMulta.Monto);
            cmd.Parameters.AddWithValue("id", entity.Id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EliminarAsync(int id)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "DELETE FROM \"Prestamo\" WHERE \"IdPrestamo\" = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<Prestamo>> ObtenerActivosPorUsuarioAsync(int idUsuario)
        {
            var lista = new List<Prestamo>();
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(@"
                SELECT * FROM ""Prestamo""
                WHERE ""IdUsuario"" = @id AND ""Estado"" = 'ACTIVO'", conn);
            cmd.Parameters.AddWithValue("id", idUsuario);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync()) lista.Add(MapearPrestamo(reader));
            return lista;
        }

        public async Task<IEnumerable<Prestamo>> ObtenerVencidosAsync()
        {
            var lista = new List<Prestamo>();
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(@"
                SELECT * FROM ""Prestamo""
                WHERE ""Estado"" = 'ACTIVO' AND ""FechaLimite"" < @hoy", conn);
            cmd.Parameters.AddWithValue("hoy", DateTime.Now.Date);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync()) lista.Add(MapearPrestamo(reader));
            return lista;
        }

        public async Task<Prestamo?> ObtenerConDetallesAsync(int id)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();

            await using var cmdP = new NpgsqlCommand(
                "SELECT * FROM \"Prestamo\" WHERE \"IdPrestamo\" = @id", conn);
            cmdP.Parameters.AddWithValue("id", id);
            await using var readerP = await cmdP.ExecuteReaderAsync();
            if (!await readerP.ReadAsync()) return null;
            var prestamo = MapearPrestamo(readerP);
            await readerP.CloseAsync();

            await using var cmdD = new NpgsqlCommand(
                "SELECT * FROM \"DetallePrestamo\" WHERE \"IdPrestamo\" = @id", conn);
            cmdD.Parameters.AddWithValue("id", id);
            await using var readerD = await cmdD.ExecuteReaderAsync();
            while (await readerD.ReadAsync())
                prestamo.AgregarDetalle(
                    Convert.ToInt32(readerD["IdRecurso"]),
                    Convert.ToInt32(readerD["Cantidad"]));

            return prestamo;
        }

        private static Prestamo MapearPrestamo(NpgsqlDataReader r)
        {
            var prestamo = Prestamo.Crear(
                Convert.ToInt32(r["IdUsuario"]),
                Convert.ToDateTime(r["FechaLimite"]));

            var multa = r["TotalMulta"] == DBNull.Value
                ? 0 : Convert.ToDecimal(r["TotalMulta"]);
            if (multa > 0)
                prestamo.AplicarMulta(new Dinero(multa));

            var estado = Enum.Parse<EstadoPrestamo>(r["Estado"].ToString()!);
            if (estado == EstadoPrestamo.DEVUELTO) prestamo.MarcarComoDevuelto();
            else if (estado == EstadoPrestamo.VENCIDO) prestamo.MarcarComoVencido();

            return prestamo;
        }
    }
}
