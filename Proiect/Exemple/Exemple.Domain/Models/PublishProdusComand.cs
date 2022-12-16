using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    public record PublishProdusComand
    {
        public PublishProdusComand(IReadOnlyCollection<UnvalidatedListaProduse> inputPretBuc)
        {
            InputPretBuc = inputPretBuc;
        }

        public IReadOnlyCollection<UnvalidatedListaProduse> InputPretBuc { get; }
    }
}
