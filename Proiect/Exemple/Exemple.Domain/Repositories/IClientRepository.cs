using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exemple.Domain.Models;

namespace Exemple.Domain.Repositories
{
    public interface IClientRepository
    {
        TryAsync<List<IdComanda>> TryGetExistingClient(IEnumerable<string> clientToCheck);
    }
}
