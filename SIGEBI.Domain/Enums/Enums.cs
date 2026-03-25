using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Enums
{
    public enum EstadoPrestamo
    {
        ACTIVO = 1,
        DEVUELTO = 2,
        VENCIDO = 3
    }

    public enum TipoRecurso
    {
        Libro = 1,
        Revista = 2
    }
}
