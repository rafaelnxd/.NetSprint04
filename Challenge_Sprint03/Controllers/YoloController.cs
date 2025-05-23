using Challenge_Sprint03.Models;
using Challenge_Sprint03.Services;
using Microsoft.AspNetCore.Mvc;

namespace Challenge_Sprint03.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class YoloController : ControllerBase
    {
        private readonly YoloService _yoloService;

        public YoloController(YoloService yoloService)
        {
            _yoloService = yoloService;
        }

        /// <summary>
        /// Detecta objetos odontológicos (Caries, Cavity, Crack, Tooth) em uma imagem base64.
        /// </summary>
        /// <param name="request">DTO contendo a string base64 da imagem a ser processada.</param>
        /// <returns>DTO com a string base64 da imagem anotada.</returns>
        [HttpPost("detectar")]
        [ProducesResponseType(typeof(YoloResponse), StatusCodes.Status200OK)]
        public ActionResult<YoloResponse> Detectar([FromBody] YoloRequest request)
        {
            var resultBase64 = _yoloService.ProcessImage(request.Base64Image);
            return Ok(new YoloResponse { Base64Image = resultBase64 });
        }
    }
}
