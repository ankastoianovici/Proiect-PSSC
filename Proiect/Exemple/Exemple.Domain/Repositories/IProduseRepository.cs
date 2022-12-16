using Exemple.Domain.Models;
using LanguageExt;
using System.Collections.Generic;
using static Exemple.Domain.Models.Cos;

namespace Exemple.Domain.Repositories
{
    public interface IProduseRepository
    {
        TryAsync<List<CalculateListaProduse>> TryGetExistingProduse();

        TryAsync<Unit> TrySaveProduse(PublicatCos produse);
    }
}

