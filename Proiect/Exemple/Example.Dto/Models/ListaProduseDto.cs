using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Dto.Models
{
   
    public record ListaProduseDto
    {
        public string IdOm { get; init; }
        public string IdComanda { get; init; }
        public decimal Cantitate { get; init; }
        public decimal Pretbuc { get; init; }
        public decimal PretFinal { get; init; }
        public string Adresa { get; init; }
    }
}
