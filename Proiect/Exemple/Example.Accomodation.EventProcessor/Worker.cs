using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Example.Events;

namespace Example.Accomodation.EventProcessor
{
    internal class Worker : IHostedService
    {
        private readonly IEventListener eventListener;

        public Worker(IEventListener eventListener)
        {
            this.eventListener = eventListener;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Worker started...");
            return eventListener.StartAsync("cos", "accomodation", cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Worker stoped!");
            return eventListener.StopAsync(cancellationToken);
        }
    }
}
