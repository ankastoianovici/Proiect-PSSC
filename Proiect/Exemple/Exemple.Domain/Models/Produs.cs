using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Exemple.Domain.Models
{
    public record Produs
    {
        public decimal Value { get; }

        public Produs(decimal value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidProdusException($"{value:0.##} is an invalid grade value.");
            }
        }

        public static Produs operator *(Produs a, Produs b) => new Produs(a.Value + b.Value);


        public Produs Round()
        {
            var roundedValue = Math.Round(Value);
            return new Produs(roundedValue);
        }

        public override string ToString()
        {
            return $"{Value:0.##}";
        }

        public static Option<Produs> TryParseProdus(decimal numericProdus)
        {
            if (IsValid(numericProdus))
            {
                return Some<Produs>(new(numericProdus));
            }
            else
            {
                return None;
            }
        }

        public static Option<Produs> TryParseProdus(string ProdusString)
        {
            if (decimal.TryParse(ProdusString, out decimal numericProdus) && IsValid(numericProdus))
            {
                return Some<Produs>(new(numericProdus));
            }
            else
            {
                return None;
            }
        }

        private static bool IsValid(decimal numericProdus) => numericProdus > 0 && numericProdus <= 100000;
    }
}
