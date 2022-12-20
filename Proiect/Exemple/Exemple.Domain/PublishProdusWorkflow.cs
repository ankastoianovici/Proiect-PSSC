using Exemple.Domain.Models;
using static Exemple.Domain.Models.CosPublishedEvent;
using static Exemple.Domain.ProdusOperation;
using System;
using static Exemple.Domain.Models.Cos;
using LanguageExt;
using System.Threading.Tasks;
using System.Collections.Generic;
using Exemple.Domain.Repositories;
using System.Linq;
using static LanguageExt.Prelude;
using Microsoft.Extensions.Logging;
using Example.Events;
using Example.Dto.Events;
using Example.Dto.Models;

namespace Exemple.Domain
{
    public class PublishProdusWorkflow
    {
        private readonly IClientRepository clientRepository;
        private readonly IProduseRepository produseRepository;
        private readonly ILogger<PublishProdusWorkflow> logger;
        private readonly IEventSender eventSender;

        public PublishProdusWorkflow(IClientRepository clientRepository, IProduseRepository produseRepository,
                                    ILogger<PublishProdusWorkflow> logger, IEventSender eventSender)
        {
            this.clientRepository = clientRepository;
            this.produseRepository = produseRepository;
            this.logger = logger;
            this.eventSender = eventSender;
        }
        public async Task<ICosPublishedEvent> ExecuteAsync(PublishProdusComand command)
        {
            NevalidatCos unvalidatedprodus = new NevalidatCos(command.InputPretBuc);

            var result = from client in clientRepository.TryGetExistingClient(unvalidatedprodus.ListaProduse.Select(prod => prod.IdComanda))
                                          .ToEither(ex => new FailedCos(unvalidatedprodus.ListaProduse, ex) as ICos)
                         from existingProdus in produseRepository.TryGetExistingProduse()
                                          .ToEither(ex => new FailedCos(unvalidatedprodus.ListaProduse, ex) as ICos)
                         let checkProdusExists = (Func<IdComanda, Option<IdComanda>>)(clienti => CheckProduseExists(client, clienti))
                         from publishedProduse in ExecuteWorkflowAsync(unvalidatedprodus, existingProdus, checkProdusExists).ToAsync()
                         from saveResult in produseRepository.TrySaveProduse(publishedProduse)
                                          .ToEither(ex => new FailedCos(unvalidatedprodus.ListaProduse, ex) as ICos)
                         let produse = publishedProduse.ListaProduse.Select(prod => new PublishProduse(
                                                        prod.IdComanda,
                                                        Pretbuc: prod.Pretbuc,
                                                        Cantitate: prod.Cantitate,
                                                        PretFinal: prod.PretFinal,
                                                        Adresa: prod.Adresa))
                         let successfulEvent = new CosPublishScucceededEvent(produse, publishedProduse.PublishedDate)
                         let eventToPublish = new ProdusePublishedEvent()
                         {
                             Produse = produse.Select(g => new ListaProduseDto()
                             {
                                 IdClient = g.IdComanda.Value, 
                                 IdComanda = g.IdComanda.Value,
                                 Pretbuc = g.Pretbuc.Value,
                                 Cantitate = g.Cantitate.Value,
                                 PretFinal = g.PretFinal.Value,
                                 Adresa = g.Adresa.Value

                             }).ToList()
                         }
                         from publishEventResult in eventSender.SendAsync("produse", eventToPublish)
                                              .ToEither(ex => new FailedCos(unvalidatedprodus.ListaProduse, ex) as ICos)
                         select successfulEvent;

            return await result.Match(
                    Left: produse => GenerateFailedEvent(produse) as ICosPublishedEvent,
                    Right: publishedProduse => publishedProduse
                );
        }
        private async Task<Either<ICos, PublicatCos>> ExecuteWorkflowAsync(NevalidatCos unvalidatedproduse,
                                                                                          IEnumerable<CalculateListaProduse> existingProduse,
                                                                                          Func<IdComanda, Option<IdComanda>> checkProdusExists)
        {

            ICos produse = await ValidateProduse(checkProdusExists, unvalidatedproduse);
            produse = CalculateFinalPrices(produse);
            produse = MergeProducts(produse, existingProduse);
            produse = PublicatCos(produse);

            return produse.Match<Either<ICos, PublicatCos>>(
                whenNevalidatCos: nevalidatcos => Left(nevalidatcos as ICos),
                whenCalculCos: calculatcos => Left(calculatcos as ICos),
                whenInvalidCos: invalidcos => Left(invalidcos as ICos),
                whenFailedCos: failedcos => Left(failedcos as ICos),
                whenValidatCos: validatcos => Left(validatcos as ICos),
                whenPublicatCos: publicatcos => Right(publicatcos)
            );
        }

        private Option<IdComanda> CheckProduseExists(IEnumerable<IdComanda> clienti, IdComanda IdComanda)
        {
            if (clienti.Any(s => s == IdComanda))
            {
                return Some(IdComanda);
            }
            else
            {
                return None;
            }
        }
        private CosPublishFaildEvent GenerateFailedEvent(ICos PretBuc) =>
            PretBuc.Match<CosPublishFaildEvent>(
                whenNevalidatCos: nevalidatcos => new($"Invalid state {nameof(nevalidatcos)}"),
                whenInvalidCos: invalidcos => new(invalidcos.Reason),
                whenValidatCos: validcos => new($"Invalid state {nameof(validcos)}"),
                whenFailedCos: failedcos =>
                {
                    logger.LogError(failedcos.Exception, failedcos.Exception.Message);
                    return new(failedcos.Exception.Message);
                },
                whenCalculCos: calculatcos => new($"Invalid state {nameof(calculatcos)}"),
                whenPublicatCos: publicatcos => new($"Invalid state {nameof(publicatcos)}"));
    }
}
    
