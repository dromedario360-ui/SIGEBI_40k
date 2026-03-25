using SIGEBI.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.ValueObjects
{
    public class Dinero : ValueObject
    {
        public decimal Monto { get; }
        public string Moneda { get; }

        private Dinero() { Moneda = string.Empty; }

        public Dinero(decimal monto, string moneda = "DOP")
        {
            if (monto < 0)
                throw new DomainException("El monto no puede ser negativo.");
            if (string.IsNullOrWhiteSpace(moneda))
                throw new DomainException("La moneda es obligatoria.");

            Monto = monto;
            Moneda = moneda.ToUpperInvariant().Trim();
        }

        public Dinero Sumar(Dinero otro)
        {
            if (Moneda != otro.Moneda)
                throw new DomainException("No se pueden sumar montos de distintas monedas.");
            return new Dinero(Monto + otro.Monto, Moneda);
        }

        public Dinero Multiplicar(int factor)
        {
            if (factor < 0)
                throw new DomainException("El factor no puede ser negativo.");
            return new Dinero(Monto * factor, Moneda);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Monto;
            yield return Moneda;
        }

        public override string ToString() => $"{Moneda} {Monto:F2}";
    }
}
