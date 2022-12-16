using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    [AsChoice]

    public static partial class Cos
    {
        public interface ICos { }
        public record NevalidatCos : ICos
        {
            public NevalidatCos(IReadOnlyCollection<UnvalidatedListaProduse> listaproduse)
            {
                ListaProduse = listaproduse;
            }
            public IReadOnlyCollection<UnvalidatedListaProduse> ListaProduse { get; }
        }
        public record InvalidCos : ICos
        {
            internal InvalidCos(IReadOnlyCollection<UnvalidatedListaProduse> listaproduse, string reason)
            {
                ListaProduse = listaproduse;
                Reason = reason;
            }
            public IReadOnlyCollection<UnvalidatedListaProduse> ListaProduse { get; }
            public string Reason { get; }
        }

        public record FailedCos : ICos
        {
            internal FailedCos(IReadOnlyCollection<UnvalidatedListaProduse> listaproduse, Exception exception)
            {
                ListaProduse = listaproduse;
                Exception = exception;
            }

            public IReadOnlyCollection<UnvalidatedListaProduse> ListaProduse { get; }
            public Exception Exception { get; }
        }

        public record ValidatCos : ICos
        {
            internal ValidatCos(IReadOnlyCollection<ValidateListaProduse> listaproduse)
            {
                ListaProduse = listaproduse;
            }
            public IReadOnlyCollection<ValidateListaProduse> ListaProduse { get; }
        }

        public record CalculCos : ICos
        {
            internal CalculCos(IReadOnlyCollection<CalculateListaProduse> listaproduse)
            {
                ListaProduse = listaproduse;
            }
            public IReadOnlyCollection<CalculateListaProduse> ListaProduse { get; }
        }

        public record PublicatCos : ICos
        {
            internal PublicatCos(IReadOnlyCollection<CalculateListaProduse> listaproduse, string csv, DateTime publishedDate)
            {
                ListaProduse = listaproduse;
                PublishedDate = publishedDate;
                Csv = csv;
            }

            public IReadOnlyCollection<CalculateListaProduse> ListaProduse { get; }
            public DateTime PublishedDate { get; }
            public string Csv { get; }
        }
    }
}

