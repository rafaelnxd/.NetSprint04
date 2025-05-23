using Challenge_Sprint03.Models;
using Microsoft.AspNetCore.Mvc;

namespace Challenge_Sprint03.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecomendacaoController : ControllerBase
    {
        private readonly RecomendacaoService _recomendacaoService;

        public RecomendacaoController(RecomendacaoService recomendacaoService)
        {
            _recomendacaoService = recomendacaoService;
        }

        [HttpPost]
        public async Task<ActionResult<RecomendacaoResponse>> Post([FromBody] RecomendacaoRequest request)
        {
            var resultado = await _recomendacaoService.GerarRecomendacaoAsync(request.Prompt);
            return Ok(resultado);
        }
    }
}
