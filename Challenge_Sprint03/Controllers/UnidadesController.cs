using Challenge_Sprint03.Models;
using Challenge_Sprint03.Models.DTOs;
using Challenge_Sprint03.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge_Sprint03.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UnidadesController : ControllerBase
    {
        private readonly UnidadesService _unidadesService;

        public UnidadesController(UnidadesService unidadesService)
        {
            _unidadesService = unidadesService;
        }

        // GET: api/Unidades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UnidadeResponseDTO>>> GetUnidades()
        {
            var unidades = await _unidadesService.GetAllAsync();
            var unidadesDTO = unidades.Select(u => new UnidadeResponseDTO
            {
                UnidadeId = u.UnidadeId,
                Nome = u.Nome,
                Estado = u.Estado,
                Cidade = u.Cidade,
                Endereco = u.Endereco
            });
            return Ok(unidadesDTO);
        }

        // GET: api/Unidades/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UnidadeResponseDTO>> GetUnidade(int id)
        {
            var unidade = await _unidadesService.GetByIdAsync(id);
            if (unidade == null)
                return NotFound($"Unidade com ID {id} não encontrada");

            var unidadeDTO = new UnidadeResponseDTO
            {
                UnidadeId = unidade.UnidadeId,
                Nome = unidade.Nome,
                Estado = unidade.Estado,
                Cidade = unidade.Cidade,
                Endereco = unidade.Endereco
            };
            return Ok(unidadeDTO);
        }

        // POST: api/Unidades
        [HttpPost]
        public async Task<ActionResult<UnidadeResponseDTO>> PostUnidade([FromBody] UnidadeCreateDTO unidadeCreateDTO)
        {
            if (string.IsNullOrWhiteSpace(unidadeCreateDTO.Endereco))
                return BadRequest("Endereço é obrigatório");

            var unidade = new Unidade
            {
                Nome = unidadeCreateDTO.Nome,
                Estado = unidadeCreateDTO.Estado,
                Cidade = unidadeCreateDTO.Cidade,
                Endereco = unidadeCreateDTO.Endereco
            };

            await _unidadesService.CreateUnidadeAsync(unidade);

            var unidadeResponse = new UnidadeResponseDTO
            {
                UnidadeId = unidade.UnidadeId,
                Nome = unidade.Nome,
                Estado = unidade.Estado,
                Cidade = unidade.Cidade,
                Endereco = unidade.Endereco
            };

            return CreatedAtAction(nameof(GetUnidade), new { id = unidade.UnidadeId }, unidadeResponse);
        }

        // PUT: api/Unidades/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUnidade(int id, [FromBody] UnidadeUpdateDTO unidadeUpdateDTO)
        {
            if (id != unidadeUpdateDTO.UnidadeId)
                return BadRequest("O ID da unidade não corresponde");

            var unidade = new Unidade
            {
                UnidadeId = unidadeUpdateDTO.UnidadeId,
                Nome = unidadeUpdateDTO.Nome,
                Estado = unidadeUpdateDTO.Estado,
                Cidade = unidadeUpdateDTO.Cidade,
                Endereco = unidadeUpdateDTO.Endereco
            };

            await _unidadesService.UpdateUnidadeAsync(unidade);
            return NoContent();
        }

        // DELETE: api/Unidades/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnidade(int id)
        {
            var unidade = await _unidadesService.GetByIdAsync(id);
            if (unidade == null)
                return NotFound($"Unidade com ID {id} não encontrada");

            await _unidadesService.DeleteUnidadeAsync(id);
            return NoContent();
        }
    }
}
