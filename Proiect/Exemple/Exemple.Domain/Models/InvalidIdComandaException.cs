using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Exemple.Domain.Models
{
    [Serializable]
    internal class InvalidIdComandaException : Exception
    {
        public InvalidIdComandaException()
        {
        }

        public InvalidIdComandaException(string? message) : base(message)
        {
        }

        public InvalidIdComandaException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidIdComandaException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
