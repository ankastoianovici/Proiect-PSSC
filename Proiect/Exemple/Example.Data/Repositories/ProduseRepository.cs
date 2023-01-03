using Exemple.Domain.Models;
using LanguageExt;
using Example.Data.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using static LanguageExt.Prelude;
using Exemple.Domain.Repositories;
using static Exemple.Domain.Models.Cos;

namespace Example.Data.Repositories
{
    public class ProduseRepository : IProduseRepository
    {
        private readonly ProduseContext dbContext;

        public ProduseRepository(ProduseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public TryAsync<List<CalculateListaProduse>> TryGetExistingProduse() => async () => (await (
                          from g in dbContext.produse
                          join s in dbContext.utilizatori on g.UtilizatorId equals s.UtilizatorId
                          select new { s.RegistrationNumber, g.ProdusId, g.PretBuc, g.Cantitate, g.PretFinal,g.Adresa })
                          .AsNoTracking()
                          .ToListAsync())
                          .Select(result => new CalculateListaProduse(
                                                    IdComanda: new(result.RegistrationNumber),
                                                    Pretbuc: new(result.PretBuc ?? 0m),
                                                    Cantitate: new(result.Cantitate ?? 0m),
                                                    PretFinal: new(result.PretFinal ?? 0m),
                                                    Adresa: new(result.Adresa))
                          {
                              ProdusId = result.ProdusId
                          })
                          .ToList();

        public TryAsync<Unit> TrySaveProduse(PublicatCos produse) => async () =>
        {
            var utilizator = (await dbContext.utilizatori.ToListAsync()).ToLookup(utilizator => utilizator.RegistrationNumber);
            var nouprodus = produse.ListaProduse
                                    .Where(g => g.IsUpdated && g.ProdusId == 0)
                                    .Select(g => new ProdusDto()
                                    {
                                        ProdusId = utilizator[g.IdComanda.Value].Single().UtilizatorId,
                                        PretBuc = g.Pretbuc.Value,
                                        Cantitate = g.Cantitate.Value,
                                        PretFinal = g.PretFinal.Value,
                                    });
            var updatedproduse = produse.ListaProduse.Where(g => g.IsUpdated && g.ProdusId > 0)
                                    .Select(g => new ProdusDto()
                                    {
                                        ProdusId = g.ProdusId,
                                        UtilizatorId = utilizator[g.IdComanda.Value].Single().UtilizatorId,
                                        PretBuc = g.Pretbuc.Value,
                                        Cantitate = g.Cantitate.Value,
                                        PretFinal = g.PretFinal.Value,
                                    });

            dbContext.AddRange(nouprodus);
            foreach (var entity in updatedproduse)
            {
                dbContext.Entry(entity).State = EntityState.Modified;
            }

            await dbContext.SaveChangesAsync();

            return unit;
        };
    }
}
