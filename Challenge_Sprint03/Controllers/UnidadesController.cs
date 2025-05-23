using Challenge_Sprint03.Models;
using Challenge_Sprint03.Models.DTOs;
using Challenge_Sprint03.Services;
using Microsoft.AspNetCore.Http;
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

        /// <summary>
        /// Retorna todas as unidades cadastradas.
        /// </summary>
        /// <returns>Lista de unidades.</returns>
        /// <response code="200">Unidades retornadas com sucesso.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UnidadeResponseDTO>), StatusCodes.Status200OK)]
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

        /// <summary>
        /// Retorna uma unidade específica pelo ID.
        /// </summary>
        /// <param name="id">ID da unidade.</param>
        /// <returns>Dados da unidade.</returns>
        /// <response code="200">Unidade encontrada.</response>
        /// <response code="404">Unidade não encontrada.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UnidadeResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Cria uma nova unidade.
        /// </summary>
        /// <param name="unidadeCreateDTO">Dados da nova unidade.</param>
        /// <returns>Unidade criada.</returns>
        /// <response code="201">Unidade criada com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        [HttpPost]
        [ProducesResponseType(typeof(UnidadeResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Atualiza uma unidade existente.
        /// </summary>
        /// <param name="id">ID da unidade.</param>
        /// <param name="unidadeUpdateDTO">Dados atualizados da unidade.</param>
        /// <returns>Status da operação.</returns>
        /// <response code="204">Atualização bem-sucedida.</response>
        /// <response code="400">Dados inválidos.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Remove uma unidade pelo ID.
        /// </summary>
        /// <param name="id">ID da unidade.</param>
        /// <returns>Status da operação.</returns>
        /// <response code="204">Unidade removida com sucesso.</response>
        /// <response code="404">Unidade não encontrada.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
