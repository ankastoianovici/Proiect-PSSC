using Exemple.Domain;
using Exemple.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;
using Example.Api.Models;
using Exemple.Domain.Models;

namespace Example.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CosController : ControllerBase
    {
        private ILogger<CosController> logger;

        public CosController(ILogger<CosController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCos([FromServices] IProduseRepository cosRepository) =>
            await cosRepository.TryGetExistingProduse().Match(
               Succ: GetAllCosHandleSuccess,
               Fail: GetAllCosHandleError
            );

        private ObjectResult GetAllCosHandleError(Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return base.StatusCode(StatusCodes.Status500InternalServerError, "UnexpectedError");
        }

        private OkObjectResult GetAllCosHandleSuccess(List<Exemple.Domain.Models.CalculateListaProduse> cos) =>
        Ok(cos.Select(cos => new
        {
            IdComanda = cos.IdComanda.Value,
            cos.Cantitate,
            cos.Pretbuc,
            cos.PretFinal,
            cos.Adresa
        }));

        [HttpPost]
        public async Task<IActionResult> PublishCos([FromServices] PublishProdusWorkflow publishCosWorkflow, [FromBody] InputCos[] cos)
        {
            var unvalidatedCos = cos.Select(MapInputCosToUnvalidatedCos)
                                          .ToList()
                                          .AsReadOnly();
            PublishProdusComand command = new(unvalidatedCos);
            var result = await publishCosWorkflow.ExecuteAsync(command);
            return result.Match<IActionResult>(
                whenCosPublishFaildEvent: failedEvent => StatusCode(StatusCodes.Status500InternalServerError, failedEvent.Reason),
                whenCosPublishScucceededEvent: successEvent => Ok()
            );
        }

        private static UnvalidatedListaProduse MapInputCosToUnvalidatedCos(InputCos cos) => new UnvalidatedListaProduse(
            IdComanda: cos.RegistrationNumber,
            Cantitate: cos.Cant,
            PretBuc: cos.Pretb,
            Adresa: cos.Adresa);
    }
}
