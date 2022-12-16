using Exemple.Domain.Models;
using static LanguageExt.Prelude;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Exemple.Domain.Models.Cos;
using System.Threading.Tasks;
using System.Diagnostics;
//using Exemple.Domain.Models;

namespace Exemple.Domain
{
    public static class ProdusOperation
    {
        public static void Main(string[] args)
        {
        }
        public static Task<ICos> ValidateProduse(Func<IdComanda, Option<IdComanda>> checkProductExists, NevalidatCos cos) =>
           cos.ListaProduse
                     .Select(ValidateProducts(checkProductExists))
                     .Aggregate(CreateEmptyValatedProduseList().ToAsync(), ReduceValidProduse)
                     .MatchAsync(
                           Right: validatedProduse => new ValidatCos(validatedProduse),
                           LeftAsync: errorMessage => Task.FromResult((ICos)new InvalidCos(cos.ListaProduse, errorMessage))
                     );

        private static Func<UnvalidatedListaProduse, EitherAsync<string, ValidateListaProduse>> ValidateProducts(Func<IdComanda, Option<IdComanda>> checkProductExists) =>
            unvalidatedProduct => ValidateStudentGrade(checkProductExists, unvalidatedProduct);

        private static EitherAsync<string, ValidateListaProduse> ValidateStudentGrade(Func<IdComanda, Option<IdComanda>> checkProductExists, UnvalidatedListaProduse unvalidatedProduse) =>
            from pretbuc in Produs.TryParseProdus(unvalidatedProduse.PretBuc)
                                   .ToEitherAsync($"Invalid pret ({unvalidatedProduse.IdComanda}, {unvalidatedProduse.PretBuc})")
            from cantitate in Produs.TryParseProdus(unvalidatedProduse.Cantitate)
                                   .ToEitherAsync($"Invalid cantitate ({unvalidatedProduse.IdComanda}, {unvalidatedProduse.Cantitate})")
            from idcomanda in IdComanda.TryParse(unvalidatedProduse.IdComanda)
                                   .ToEitherAsync($"Invalid id ({unvalidatedProduse.IdComanda})")
            from adresa in AdresaPlata.TryParseAdresa(unvalidatedProduse.Adresa)
                                    .ToEitherAsync($"Invalid cantitate ({unvalidatedProduse.IdComanda}, {unvalidatedProduse.Adresa})")
            from productExists in checkProductExists(idcomanda)
                                   .ToEitherAsync($"Produs {idcomanda.Value} does not exist.")
            select new ValidateListaProduse(idcomanda, cantitate, pretbuc, adresa);

        private static Either<string, List<ValidateListaProduse>> CreateEmptyValatedProduseList() =>
            Right(new List<ValidateListaProduse>());

        private static EitherAsync<string, List<ValidateListaProduse>> ReduceValidProduse(EitherAsync<string, List<ValidateListaProduse>> acc, EitherAsync<string, ValidateListaProduse> next) =>
            from list in acc
            from nextGrade in next
            select list.AppendValidGrade(nextGrade);

        private static List<ValidateListaProduse> AppendValidGrade(this List<ValidateListaProduse> list, ValidateListaProduse validGrade)
        {
            list.Add(validGrade);
            return list;
        }

        public static ICos CalculateFinalPrices(ICos cos) => cos.Match(
            whenNevalidatCos: nevalidatcos => nevalidatcos,
            whenInvalidCos: invalidcos => invalidcos,
            whenFailedCos: failedcos => failedcos,
            whenCalculCos: calculatcos => calculatcos,
            whenPublicatCos: publicatcos => publicatcos,
            whenValidatCos: CalculatPretFinal
        );

        private static ICos CalculatPretFinal(ValidatCos validCos) =>
            new CalculCos(validCos.ListaProduse
                                                    .Select(CalculatCosPretFinal)
                                                    .ToList()
                                                    .AsReadOnly());

        private static CalculateListaProduse CalculatCosPretFinal(ValidateListaProduse validProduse) =>
            new CalculateListaProduse(validProduse.IdComanda,
                                      validProduse.Cantitate,
                                      validProduse.Pret_Buc,
                                      validProduse.Pret_Buc * validProduse.Cantitate,
                                      validProduse.Adresa);

        public static ICos MergeProducts(ICos cos, IEnumerable<CalculateListaProduse> existingProducts) => cos.Match(
           whenNevalidatCos: nevalidatcos => nevalidatcos,
            whenInvalidCos: invalidcos => invalidcos,
            whenFailedCos: failedcos => failedcos,
            whenValidatCos: validatcos => validatcos,
            whenPublicatCos: publicatcos => publicatcos,
            whenCalculCos: calculatcos => MergeProducts(calculatcos.ListaProduse, existingProducts));


        private static CalculCos MergeProducts(IEnumerable<CalculateListaProduse> newList, IEnumerable<CalculateListaProduse> existingList)
        {
            var updatedAndNewProduse = newList.Select(produse => produse with { ProdusId = existingList.FirstOrDefault(g => g.IdComanda == produse.IdComanda)?.ProdusId ?? 0, IsUpdated = true });
            var oldProduse = existingList.Where(produse => !newList.Any(g => g.IdComanda == produse.IdComanda));
            var allProduse = updatedAndNewProduse.Union(oldProduse)
                                               .ToList()
                                               .AsReadOnly();
            return new CalculCos(allProduse);
        }

        public static ICos PublicatCos(ICos cos) => cos.Match(
             whenNevalidatCos: nevalidatcos => nevalidatcos,
            whenInvalidCos: invalidcos => invalidcos,
            whenFailedCos: failedcos => failedcos,
            whenValidatCos: validatcos => validatcos,
            whenPublicatCos: publicatcos => publicatcos,
            whenCalculCos: GenerateExport);

        private static ICos GenerateExport(CalculCos calculatedcos) =>
            new PublicatCos(calculatedcos.ListaProduse,
                                    calculatedcos.ListaProduse.Aggregate(new StringBuilder(), CreateCsvLine).ToString(),
                                    DateTime.Now);

        private static StringBuilder CreateCsvLine(StringBuilder export, CalculateListaProduse produse) =>
            export.AppendLine($"{produse.IdComanda.Value}, {produse.Cantitate}, {produse.Pretbuc}, {produse.PretFinal}, {produse.Adresa}");
    }
}