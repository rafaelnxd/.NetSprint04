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
    public class UsuariosController : ControllerBase
    {
        private readonly UsuariosService _usuariosService;

        public UsuariosController(UsuariosService usuariosService)
        {
            _usuariosService = usuariosService;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioResponseDTO>>> GetUsuarios()
        {
            var usuarios = await _usuariosService.GetAllAsync();
            var usuariosDTO = usuarios.Select(u => new UsuarioResponseDTO
            {
                UsuarioId = u.UsuarioId,
                Email = u.Email,
                Nome = u.Nome,
                DataCadastro = u.DataCadastro,
                PontosRecompensa = u.PontosRecompensa
            });
            return Ok(usuariosDTO);
        }

        // GET: api/Usuarios/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioResponseDTO>> GetUsuario(int id)
        {
            var usuario = await _usuariosService.GetByIdAsync(id);
            if (usuario == null)
                return NotFound($"Usuário com ID {id} não encontrado");

            var usuarioDTO = new UsuarioResponseDTO
            {
                UsuarioId = usuario.UsuarioId,
                Email = usuario.Email,
                Nome = usuario.Nome,
                DataCadastro = usuario.DataCadastro,
                PontosRecompensa = usuario.PontosRecompensa
            };
            return Ok(usuarioDTO);
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<UsuarioResponseDTO>> PostUsuario([FromBody] UsuarioCreateDTO usuarioCreateDTO)
        {
            try
            {
                var usuario = new Usuario
                {
                    Email = usuarioCreateDTO.Email,
                    Nome = usuarioCreateDTO.Nome,
                    Senha = usuarioCreateDTO.Senha
                };

                var novoUsuario = await _usuariosService.CreateUsuarioAsync(usuario);

                var usuarioResponse = new UsuarioResponseDTO
                {
                    UsuarioId = novoUsuario.UsuarioId,
                    Email = novoUsuario.Email,
                    Nome = novoUsuario.Nome,
                    DataCadastro = novoUsuario.DataCadastro,
                    PontosRecompensa = novoUsuario.PontosRecompensa
                };

                return CreatedAtAction(nameof(GetUsuario), new { id = usuarioResponse.UsuarioId }, usuarioResponse);
            }
            catch (System.Exception ex)
            {
                return BadRequest($"Erro ao criar usuário: {ex.Message}");
            }
        }

        // PUT: api/Usuarios/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, [FromBody] UsuarioUpdateDTO usuarioUpdateDTO)
        {
            if (id != usuarioUpdateDTO.UsuarioId)
                return BadRequest("O ID do usuário não corresponde");

            var usuario = new Usuario
            {
                UsuarioId = usuarioUpdateDTO.UsuarioId,
                Email = usuarioUpdateDTO.Email,
                Nome = usuarioUpdateDTO.Nome,
                Senha = usuarioUpdateDTO.Senha
            };

            await _usuariosService.UpdateUsuarioAsync(usuario);
            return NoContent();
        }

        // DELETE: api/Usuarios/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _usuariosService.GetByIdAsync(id);
            if (usuario == null)
                return NotFound($"Usuário com ID {id} não encontrado");

            await _usuariosService.DeleteUsuarioAsync(id);
            return NoContent();
        }

        // PATCH: api/Usuarios/{id}/pontos
        [HttpPatch("{id}/pontos")]
        public async Task<IActionResult> AtualizarPontos(int id, [FromBody] int pontos)
        {
            try
            {
                await _usuariosService.UpdatePontosAsync(id, pontos);
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return BadRequest($"Erro ao atualizar pontos: {ex.Message}");
            }
        }
    }
}
