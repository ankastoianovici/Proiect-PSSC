using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    public record UnvalidatedListaProduse(string IdComanda, decimal PretBuc, decimal Cantitate, string Adresa);
    
}
