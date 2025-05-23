using Challenge_Sprint03.Models;
using Challenge_Sprint03.Models.DTOs;
using Challenge_Sprint03.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Challenge_Sprint03.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistroHabitoController : ControllerBase
    {
        private readonly RegistroHabitoService _registroHabitoService;

        public RegistroHabitoController(RegistroHabitoService registroHabitoService)
        {
            _registroHabitoService = registroHabitoService;
        }

        /// <summary>
        /// Retorna todos os registros de hábitos.
        /// </summary>
        /// <returns>Lista de registros.</returns>
        /// <response code="200">Registros retornados com sucesso.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RegistroHabitoResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RegistroHabitoResponseDTO>>> GetRegistros()
        {
            var registrosDTO = await _registroHabitoService.GetAllAsync();
            return Ok(registrosDTO);
        }

        /// <summary>
        /// Retorna um registro específico pelo ID.
        /// </summary>
        /// <param name="id">ID do registro.</param>
        /// <returns>Dados do registro.</returns>
        /// <response code="200">Registro encontrado.</response>
        /// <response code="404">Registro não encontrado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RegistroHabitoResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RegistroHabitoResponseDTO>> GetRegistro(int id)
        {
            var registro = await _registroHabitoService.GetByIdAsync(id);
            if (registro == null)
                return NotFound($"Registro com ID {id} não encontrado");

            var registroDTO = new RegistroHabitoResponseDTO
            {
                Id = registro.Id,
                Data = registro.Data,
                Observacoes = registro.Observacoes,
                HabitoDescricao = registro.Habito != null ? registro.Habito.Descricao : null
            };
            return Ok(registroDTO);
        }

        /// <summary>
        /// Cria um novo registro de hábito.
        /// </summary>
        /// <param name="registroCreateDTO">Dados do novo registro.</param>
        /// <returns>Registro criado.</returns>
        /// <response code="201">Registro criado com sucesso.</response>
        [HttpPost]
        [ProducesResponseType(typeof(RegistroHabitoResponseDTO), StatusCodes.Status201Created)]
        public async Task<ActionResult<RegistroHabitoResponseDTO>> PostRegistro([FromBody] RegistroHabitoCreateDTO registroCreateDTO)
        {
            var registro = new RegistroHabito
            {
                HabitoId = registroCreateDTO.HabitoId,
                Imagem = registroCreateDTO.Imagem,
                Observacoes = registroCreateDTO.Observacoes
            };

            await _registroHabitoService.CreateRegistroAsync(registro);

            var registroResponse = new RegistroHabitoResponseDTO
            {
                Id = registro.Id,
                Data = registro.Data,
                Observacoes = registro.Observacoes,
                HabitoDescricao = registro.Habito != null ? registro.Habito.Descricao : null
            };

            return CreatedAtAction(nameof(GetRegistro), new { id = registro.Id }, registroResponse);
        }

        /// <summary>
        /// Atualiza um registro de hábito existente.
        /// </summary>
        /// <param name="id">ID do registro.</param>
        /// <param name="registroUpdateDTO">Dados atualizados do registro.</param>
        /// <returns>Status da operação.</returns>
        /// <response code="204">Atualização bem-sucedida.</response>
        /// <response code="400">ID inválido.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutRegistro(int id, [FromBody] RegistroHabitoUpdateDTO registroUpdateDTO)
        {
            if (id != registroUpdateDTO.Id)
                return BadRequest("O ID do registro não corresponde");

            var registro = new RegistroHabito
            {
                Id = registroUpdateDTO.Id,
                HabitoId = registroUpdateDTO.HabitoId,
                Imagem = registroUpdateDTO.Imagem,
                Observacoes = registroUpdateDTO.Observacoes
            };

            await _registroHabitoService.UpdateRegistroAsync(registro);
            return NoContent();
        }

        /// <summary>
        /// Exclui um registro de hábito.
        /// </summary>
        /// <param name="id">ID do registro.</param>
        /// <returns>Status da operação.</returns>
        /// <response code="204">Registro removido com sucesso.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteRegistro(int id)
        {
            await _registroHabitoService.DeleteRegistroAsync(id);
            return NoContent();
        }
    }
}
