using LanguageExt;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Exemple.Domain.Repositories;
using Exemple.Domain.Models;


namespace Example.Data.Repositories
{
    public class OmRepository : IOmRepository
    {
        private readonly ProduseContext produseContext;

        public OmRepository(ProduseContext gradesContext)
        {
            this.produseContext = gradesContext;
        }

        public TryAsync<List<IdComanda>> TryGetExistingOm(IEnumerable<string> omToCheck) => async () =>
        {
            var oameni = await produseContext.oameni
                                              .Where(oameni => omToCheck.Contains(oameni.RegistrationNumber))
                                              .AsNoTracking()
                                              .ToListAsync();
            return oameni.Select(student => new IdComanda(student.RegistrationNumber))
                           .ToList();
        };
    }
}
