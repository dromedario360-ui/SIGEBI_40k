using SIGEBI.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.ValueObjects
{
    public class Email : ValueObject
    {
        public string Value { get; }

        private Email() { Value = string.Empty; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("El email es obligatorio.");

            var arroba = value.IndexOf('@');
            var punto = value.LastIndexOf('.');

            if (arroba <= 0 || punto <= arroba + 1 || punto >= value.Length - 1)
                throw new DomainException("El formato del email es inválido.");

            Value = value.ToLowerInvariant().Trim();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value;
    }
}
