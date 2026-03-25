using Npgsql;
using SIGEBI.Domain.Entities.Recursos;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Interfaces;
using SIGEBI.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RecursoRepository : IRecursoRepository
    {
        private readonly ConexionDb _db;

        public RecursoRepository(ConexionDb db) => _db = db;

        public async Task<RecursoBase?> ObtenerPorIdAsync(int id)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT * FROM \"RecursoBibliografico\" WHERE \"IdRecurso\" = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync()) return Mapear(reader);
            return null;
        }

        public async Task<IEnumerable<RecursoBase>> ObtenerTodosAsync()
        {
            var lista = new List<RecursoBase>();
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT * FROM \"RecursoBibliografico\"", conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync()) lista.Add(Mapear(reader));
            return lista;
        }

        public async Task AgregarAsync(RecursoBase entity)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(@"
                INSERT INTO ""RecursoBibliografico""
                (""IdCategoria"", ""Codigo"", ""Titulo"", ""Autor"", 
                 ""ISBN"", ""Stock"", ""Disponible"", ""FechaRegistro"")
                VALUES (@cat, @codigo, @titulo, @autor, 
                        @isbn, @stock, @disponible, @fecha)", conn);
            cmd.Parameters.AddWithValue("cat", entity.IdCategoria);
            cmd.Parameters.AddWithValue("codigo", entity.Codigo);
            cmd.Parameters.AddWithValue("titulo", entity.Titulo);
            cmd.Parameters.AddWithValue("autor", entity.Autor);
            cmd.Parameters.AddWithValue("isbn", (object?)entity.ISBN ?? DBNull.Value);
            cmd.Parameters.AddWithValue("stock", entity.Stock);
            cmd.Parameters.AddWithValue("disponible", entity.Disponible);
            cmd.Parameters.AddWithValue("fecha", entity.FechaRegistro);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task ActualizarAsync(RecursoBase entity)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(@"
                UPDATE ""RecursoBibliografico"" SET
                ""Titulo"" = @titulo, ""Autor"" = @autor,
                ""IdCategoria"" = @cat, ""ISBN"" = @isbn,
                ""Stock"" = @stock, ""Disponible"" = @disponible
                WHERE ""IdRecurso"" = @id", conn);
            cmd.Parameters.AddWithValue("titulo", entity.Titulo);
            cmd.Parameters.AddWithValue("autor", entity.Autor);
            cmd.Parameters.AddWithValue("cat", entity.IdCategoria);
            cmd.Parameters.AddWithValue("isbn", (object?)entity.ISBN ?? DBNull.Value);
            cmd.Parameters.AddWithValue("stock", entity.Stock);
            cmd.Parameters.AddWithValue("disponible", entity.Disponible);
            cmd.Parameters.AddWithValue("id", entity.Id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EliminarAsync(int id)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "DELETE FROM \"RecursoBibliografico\" WHERE \"IdRecurso\" = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<RecursoBase?> ObtenerPorCodigoAsync(string codigo)
        {
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT * FROM \"RecursoBibliografico\" WHERE \"Codigo\" = @codigo", conn);
            cmd.Parameters.AddWithValue("codigo", codigo);
            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync()) return Mapear(reader);
            return null;
        }

        public async Task<IEnumerable<RecursoBase>> ObtenerDisponiblesAsync()
        {
            var lista = new List<RecursoBase>();
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT * FROM \"RecursoBibliografico\" WHERE \"Disponible\" = true", conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync()) lista.Add(Mapear(reader));
            return lista;
        }

        public async Task<IEnumerable<RecursoBase>> ObtenerPorCategoriaAsync(int idCategoria)
        {
            var lista = new List<RecursoBase>();
            await using var conn = _db.CrearConexion();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT * FROM \"RecursoBibliografico\" WHERE \"IdCategoria\" = @cat", conn);
            cmd.Parameters.AddWithValue("cat", idCategoria);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync()) lista.Add(Mapear(reader));
            return lista;
        }

        private static RecursoBase Mapear(NpgsqlDataReader r)
        {
            var tipo = r["Tipo"] != DBNull.Value
                ? Enum.Parse<TipoRecurso>(r["Tipo"].ToString()!)
                : TipoRecurso.Libro;

            var idCat = Convert.ToInt32(r["IdCategoria"]);
            var codigo = r["Codigo"].ToString()!;
            var titulo = r["Titulo"].ToString()!;
            var autor = r["Autor"].ToString()!;
            var stock = Convert.ToInt32(r["Stock"]);
            var isbn = r["ISBN"] == DBNull.Value ? null : r["ISBN"].ToString();

            if (tipo == TipoRecurso.Revista)
                return Revista.Crear(idCat, codigo, titulo, autor, stock, isbn);

            return Libro.Crear(idCat, codigo, titulo, autor, stock, isbn);
        }
    }
}
