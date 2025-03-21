using Challenge_Sprint03.Models;
using Challenge_Sprint03.Models.DTOs;
using Challenge_Sprint03.Services;
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

        // GET: api/RegistroHabito
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegistroHabitoResponseDTO>>> GetRegistros()
        {
            // Supondo que o service já retorna os DTOs, não é necessário remapear
            var registrosDTO = await _registroHabitoService.GetAllAsync();
            return Ok(registrosDTO);
        }

        // GET: api/RegistroHabito/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<RegistroHabitoResponseDTO>> GetRegistro(int id)
        {
            // Aqui, se o service GetByIdAsync retorna a entidade, mapeamos para o DTO:
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

        // POST: api/RegistroHabito
        [HttpPost]
        public async Task<ActionResult<RegistroHabitoResponseDTO>> PostRegistro([FromBody] RegistroHabitoCreateDTO registroCreateDTO)
        {
            var registro = new RegistroHabito
            {
                HabitoId = registroCreateDTO.HabitoId,
                Imagem = registroCreateDTO.Imagem,
                Observacoes = registroCreateDTO.Observacoes
                // A propriedade Data será definida no service
            };

            await _registroHabitoService.CreateRegistroAsync(registro);

            // Aqui, fazemos o mapeamento da entidade para o DTO
            var registroResponse = new RegistroHabitoResponseDTO
            {
                Id = registro.Id,
                Data = registro.Data,
                Observacoes = registro.Observacoes,
                HabitoDescricao = registro.Habito != null ? registro.Habito.Descricao : null
            };

            return CreatedAtAction(nameof(GetRegistro), new { id = registro.Id }, registroResponse);
        }

        // PUT: api/RegistroHabito/{id}
        [HttpPut("{id}")]
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

        // DELETE: api/RegistroHabito/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistro(int id)
        {
            await _registroHabitoService.DeleteRegistroAsync(id);
            return NoContent();
        }
    }
}
