using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Infrastructure.Data
{
    public class ConexionDb
    {
        private readonly string _connectionString;

        public ConexionDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public NpgsqlConnection CrearConexion()
            => new NpgsqlConnection(_connectionString);
    }
}
