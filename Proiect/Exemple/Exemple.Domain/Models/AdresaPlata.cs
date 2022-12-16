using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Exemple.Domain.Models
{
    public record AdresaPlata
    {
        private static readonly Regex ValidPattern = new("^LM[0-9]{5}$");
        public string Value { get; }
        public AdresaPlata(string value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new AdresaInvalida("");
            }

        }
        public static bool IsValid(string stringValue) => ValidPattern.IsMatch(stringValue);

        public override string ToString()
        {
            return Value;
        }
        public static Option<AdresaPlata> TryParseAdresa(string stringValue)
        {
            if (IsValid(stringValue))
            {
                return Some<AdresaPlata>(new(stringValue));
            }
            else
            {
                return None;
            }

        }
    }
}
