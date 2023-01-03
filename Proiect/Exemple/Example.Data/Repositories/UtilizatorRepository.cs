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
            var oameni = await produseContext.utilizatori
                                              .Where(utilizator => utilizatorToCheck.Contains(utilizator.RegistrationNumber))
                                              .AsNoTracking()
                                              .ToListAsync();
            return utilizator.Select(utilizator => new IdComanda(utilizator.RegistrationNumber))
                           .ToList();
        };
    }
}
