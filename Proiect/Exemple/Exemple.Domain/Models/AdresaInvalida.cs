using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Exemple.Domain.Models
{
    [Serializable]
    internal class AdresaInvalida : Exception
    {
        public AdresaInvalida()
        {
        }

        public AdresaInvalida(string? message) : base(message)
        {
        }

        public AdresaInvalida(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected AdresaInvalida(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
