using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Exemple.Domain.Models.CosPublishedEvent;
using Example.Dto.Events;
using Example.Events;
using Example.Events.Models;

namespace Example.Accomodation.EventProcessor
{
    internal class ProdusePublishedEventHandler : AbstractEventHandler<ProdusePublishedEvent>
    {
        public override string[] EventTypes => new string[] { typeof(ProdusePublishedEvent).Name };

        protected override Task<EventProcessingResult> OnHandleAsync(ProdusePublishedEvent eventData)
        {
            Console.WriteLine(eventData.ToString());
            return Task.FromResult(EventProcessingResult.Completed);
        }
    }
}
