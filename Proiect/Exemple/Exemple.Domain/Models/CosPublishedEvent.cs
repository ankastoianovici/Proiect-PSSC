using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    [AsChoice]
    public static partial class CosPublishedEvent
    {
        public interface ICosPublishedEvent { }

        public record CosPublishScucceededEvent : ICosPublishedEvent
        {
            public IEnumerable<PublishProduse> Produs { get; }
            public DateTime PublishedDate { get; }

            internal CosPublishScucceededEvent(IEnumerable<PublishProduse> produs, DateTime publishedDate)
            {
                Produs = produs;
                PublishedDate = publishedDate;
            }
        }

        public record CosPublishFaildEvent : ICosPublishedEvent
        {
            public string Reason { get; }

            internal CosPublishFaildEvent(string reason)
            {
                Reason = reason;
            }
        }
    }
}
