using LanguageExt;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Exemple.Domain.Repositories;
using Exemple.Domain.Models;


namespace Example.Data.Repositories
{
    public class UtilizatorRepository : IUtilizatorRepository
    {
        private readonly ProduseContext produseContext;

        public UtilizatorRepository(ProduseContext produseContext)
        {
            this.produseContext = produseContext;
        }

        public TryAsync<List<IdComanda>> TryGetExistingUtilizator(IEnumerable<string> utilizatorToCheck) => async () =>
        {
            var utilizatori = await produseContext.utilizatori
                                              .Where(utilizatori => utilizatorToCheck.Contains(utilizatori.RegistrationNumber))
                                              .AsNoTracking()
                                              .ToListAsync();
            return utilizatori.Select(utilizator => new IdComanda(utilizator.RegistrationNumber))
                           .ToList();
        };
    }
}
