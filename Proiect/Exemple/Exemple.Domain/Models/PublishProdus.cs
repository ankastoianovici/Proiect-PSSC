using Exemple.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    public record PublishProduse(IdComanda IdComanda, Produs Pretbuc, Produs Cantitate, Produs PretFinal, AdresaPlata Adresa);
 
}
