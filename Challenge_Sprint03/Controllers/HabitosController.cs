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
    public class HabitosController : ControllerBase
    {
        private readonly HabitoService _habitoService;

        public HabitosController(HabitoService habitoService)
        {
            _habitoService = habitoService;
        }

        /// <summary>
        /// Retorna todos os hábitos cadastrados.
        /// </summary>
        /// <returns>Lista de hábitos.</returns>
        /// <response code="200">Lista retornada com sucesso.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<HabitoResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<HabitoResponseDTO>>> GetHabitos()
        {
            var habitos = await _habitoService.GetAllHabitosAsync();
            var habitosDTO = habitos.Select(h => new HabitoResponseDTO
            {
                HabitoId = h.HabitoId,
                Descricao = h.Descricao,
                Tipo = h.Tipo,
                FrequenciaIdeal = h.FrequenciaIdeal
            });
            return Ok(habitosDTO);
        }

        /// <summary>
        /// Retorna um hábito específico pelo ID.
        /// </summary>
        /// <param name="id">ID do hábito.</param>
        /// <returns>Dados do hábito.</returns>
        /// <response code="200">Hábito encontrado.</response>
        /// <response code="404">Hábito não encontrado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(HabitoResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HabitoResponseDTO>> GetHabito(int id)
        {
            var habito = await _habitoService.GetHabitoByIdAsync(id);
            if (habito == null)
                return NotFound($"Hábito com ID {id} não encontrado");

            var habitoDTO = new HabitoResponseDTO
            {
                HabitoId = habito.HabitoId,
                Descricao = habito.Descricao,
                Tipo = habito.Tipo,
                FrequenciaIdeal = habito.FrequenciaIdeal
            };
            return Ok(habitoDTO);
        }

        /// <summary>
        /// Cadastra um novo hábito.
        /// </summary>
        /// <param name="habitoCreateDTO">Dados do novo hábito.</param>
        /// <returns>Hábito criado.</returns>
        /// <response code="201">Hábito criado com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        [HttpPost]
        [ProducesResponseType(typeof(HabitoResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<HabitoResponseDTO>> PostHabito([FromBody] HabitoCreateDTO habitoCreateDTO)
        {
            if (habitoCreateDTO.FrequenciaIdeal < 1 || habitoCreateDTO.FrequenciaIdeal > 7)
                return BadRequest("Frequência ideal deve ser entre 1 e 7 dias");

            var habito = new Habito
            {
                Descricao = habitoCreateDTO.Descricao,
                Tipo = habitoCreateDTO.Tipo,
                FrequenciaIdeal = habitoCreateDTO.FrequenciaIdeal
            };

            await _habitoService.CreateHabitoAsync(habito);

            var habitoResponse = new HabitoResponseDTO
            {
                HabitoId = habito.HabitoId,
                Descricao = habito.Descricao,
                Tipo = habito.Tipo,
                FrequenciaIdeal = habito.FrequenciaIdeal
            };

            return CreatedAtAction(nameof(GetHabito), new { id = habito.HabitoId }, habitoResponse);
        }

        /// <summary>
        /// Atualiza um hábito existente.
        /// </summary>
        /// <param name="id">ID do hábito a ser atualizado.</param>
        /// <param name="habitoUpdateDTO">Dados atualizados do hábito.</param>
        /// <returns>Status da operação.</returns>
        /// <response code="204">Atualização realizada com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutHabito(int id, [FromBody] HabitoUpdateDTO habitoUpdateDTO)
        {
            if (id != habitoUpdateDTO.HabitoId)
                return BadRequest("O ID do hábito não corresponde");

            if (habitoUpdateDTO.FrequenciaIdeal < 1 || habitoUpdateDTO.FrequenciaIdeal > 7)
                return BadRequest("Frequência ideal deve ser entre 1 e 7 dias");

            var habito = new Habito
            {
                HabitoId = habitoUpdateDTO.HabitoId,
                Descricao = habitoUpdateDTO.Descricao,
                Tipo = habitoUpdateDTO.Tipo,
                FrequenciaIdeal = habitoUpdateDTO.FrequenciaIdeal
            };

            await _habitoService.UpdateHabitoAsync(habito);
            return NoContent();
        }

        /// <summary>
        /// Remove um hábito existente.
        /// </summary>
        /// <param name="id">ID do hábito a ser excluído.</param>
        /// <returns>Status da operação.</returns>
        /// <response code="204">Exclusão realizada com sucesso.</response>
        /// <response code="404">Hábito não encontrado.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteHabito(int id)
        {
            var habito = await _habitoService.GetHabitoByIdAsync(id);
            if (habito == null)
                return NotFound($"Hábito com ID {id} não encontrado");

            await _habitoService.DeleteHabitoAsync(id);
            return NoContent();
        }
    }
}
