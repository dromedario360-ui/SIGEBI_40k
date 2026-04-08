using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Base
{
    public abstract class BaseEntity
    {
        public int Id { get; protected set; }

        public void EstablecerId(int id)
        {
            Id = id;
        }
    }
}
