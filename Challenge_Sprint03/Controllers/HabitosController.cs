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
    public class HabitosController : ControllerBase
    {
        private readonly HabitoService _habitoService;

        public HabitosController(HabitoService habitoService)
        {
            _habitoService = habitoService;
        }

        // GET: api/Habitos
        [HttpGet]
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

        // GET: api/Habitos/{id}
        [HttpGet("{id}")]
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

        // POST: api/Habitos
        [HttpPost]
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

        // PUT: api/Habitos/{id}
        [HttpPut("{id}")]
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

        // DELETE: api/Habitos/{id}
        [HttpDelete("{id}")]
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
