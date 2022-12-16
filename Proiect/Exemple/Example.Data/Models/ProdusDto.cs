using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Data.Models
{
    public class ProdusDto
    {
        public int ProdusId { get; set; }
        public int OmId { get; set; }
        public decimal? PretBuc { get; set; }
        public decimal? Cantitate { get; set; }
        public decimal? PretFinal { get; set; }
        public string Adresa { get; set; }
    }
}
