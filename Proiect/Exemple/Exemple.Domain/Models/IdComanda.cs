using LanguageExt;
using static LanguageExt.Prelude;
using System.Text.RegularExpressions;

namespace Exemple.Domain.Models
{
    public record IdComanda
    {
        public const string Pattern = "^[0-9]{1}";
        private static readonly Regex PatternRegex = new(Pattern);

        public string Value { get; }

        public IdComanda(string value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidIdComandaException("Id invalid");
            }
        }
        private static bool IsValid(string stringValue) => PatternRegex.IsMatch(stringValue);

        public override string ToString()
        {
            return Value;
        }

        public static Option<IdComanda> TryParse(string stringValue)
        {
            if (IsValid(stringValue))
            {
                return Some<IdComanda>(new(stringValue));
            }
            else
            {
                return None;
            }
        }

    }
}
