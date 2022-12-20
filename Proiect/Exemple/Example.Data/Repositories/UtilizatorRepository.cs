using LanguageExt;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Exemple.Domain.Repositories;
using Exemple.Domain.Models;


namespace Example.Data.Repositories
{
    public class UtilizatorRepository : IOmRepository
    {
        private readonly ProduseContext produseContext;

        public UtilizatorRepository(ProduseContext gradesContext)
        {
            this.produseContext = gradesContext;
        }

        public TryAsync<List<IdComanda>> TryGetExistingUtilizator(IEnumerable<string> utilizatorToCheck) => async () =>
        {
            var oameni = await produseContext.oameni
                                              .Where(oameni => utilizatorToCheck.Contains(oameni.RegistrationNumber))
                                              .AsNoTracking()
                                              .ToListAsync();
            return oameni.Select(student => new IdComanda(student.RegistrationNumber))
                           .ToList();
        };
    }
}
