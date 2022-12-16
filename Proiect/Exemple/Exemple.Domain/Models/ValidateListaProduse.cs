using Exemple.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    public record ValidateListaProduse(IdComanda IdComanda,Produs Pret_Buc, Produs Cantitate, AdresaPlata Adresa);
}
