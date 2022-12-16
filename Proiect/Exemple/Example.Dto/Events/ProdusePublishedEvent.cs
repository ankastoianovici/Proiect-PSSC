using Example.Dto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Dto.Events
{
    public record ProdusePublishedEvent
    {
        public List<ListaProduseDto> Produse { get; init; }
    }
}
