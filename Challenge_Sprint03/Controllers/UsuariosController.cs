using Challenge_Sprint03.Models;
using Challenge_Sprint03.Models.DTOs;
using Challenge_Sprint03.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly EmailService _emailService;
        private readonly ILogger<UsuariosController> _log;

        public UsuariosController(
            UsuariosService usuariosService,
            EmailService emailService,
            ILogger<UsuariosController> logger)
        {
            _usuariosService = usuariosService;
            _emailService = emailService;
            _log = logger;
        }

        /// <summary>
        /// Retorna todos os usuários cadastrados.
        /// </summary>
        /// <returns>Lista de usuários.</returns>
        /// <response code="200">Lista retornada com sucesso.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UsuarioResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UsuarioResponseDTO>>> GetUsuarios()
        {
            var usuarios = await _usuariosService.GetAllAsync();
            var dto = usuarios.Select(u => new UsuarioResponseDTO
            {
                UsuarioId = u.UsuarioId,
                Email = u.Email,
                Nome = u.Nome,
                DataCadastro = u.DataCadastro,
                PontosRecompensa = u.PontosRecompensa
            });
            return Ok(dto);
        }

        /// <summary>
        /// Retorna um usuário específico pelo ID.
        /// </summary>
        /// <param name="id">ID do usuário.</param>
        /// <returns>Dados do usuário.</returns>
        /// <response code="200">Usuário encontrado.</response>
        /// <response code="404">Usuário não encontrado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UsuarioResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UsuarioResponseDTO>> GetUsuario(int id)
        {
            var u = await _usuariosService.GetByIdAsync(id);
            if (u == null) return NotFound($"Usuário {id} não encontrado");

            return Ok(new UsuarioResponseDTO
            {
                UsuarioId = u.UsuarioId,
                Email = u.Email,
                Nome = u.Nome,
                DataCadastro = u.DataCadastro,
                PontosRecompensa = u.PontosRecompensa
            });
        }

        /// <summary>
        /// Cadastra um novo usuário.
        /// </summary>
        /// <param name="dto">Dados do novo usuário.</param>
        /// <returns>Usuário criado.</returns>
        /// <response code="201">Usuário criado com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        [HttpPost]
        [ProducesResponseType(typeof(UsuarioResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UsuarioResponseDTO>> PostUsuario(
            [FromBody] UsuarioCreateDTO dto)
        {
            _log.LogInformation("📥 Novo cadastro de usuário");

            var usuario = new Usuario
            {
                Email = dto.Email,
                Nome = dto.Nome,
                Senha = dto.Senha
            };

            var novo = await _usuariosService.CreateUsuarioAsync(usuario);

            var respDto = new UsuarioResponseDTO
            {
                UsuarioId = novo.UsuarioId,
                Email = novo.Email,
                Nome = novo.Nome,
                DataCadastro = novo.DataCadastro,
                PontosRecompensa = novo.PontosRecompensa
            };

            // E-mail de boas-vindas
            var html = $@"
                <h2>Bem-vindo à nossa plataforma!</h2>
                <p>Olá <strong>{respDto.Nome}</strong>, seu cadastro foi realizado com sucesso.</p>
                <p>Data de cadastro: {respDto.DataCadastro:dd/MM/yyyy}</p>
                <p><em>Obrigado por se juntar a nós!</em></p>";

            var mailRes = await _emailService.EnviarEmailAsync(
                respDto.Email,
                "Cadastro realizado com sucesso",
                html);

            if (!mailRes.Success)
            {
                _log.LogWarning("Falha ao enviar e-mail: {Status} {Body}",
                                mailRes.Status, mailRes.Body);
            }

            return CreatedAtAction(nameof(GetUsuario),
                                   new { id = respDto.UsuarioId },
                                   respDto);
        }

        /// <summary>
        /// Atualiza os dados de um usuário existente.
        /// </summary>
        /// <param name="id">ID do usuário a ser atualizado.</param>
        /// <param name="dto">Dados atualizados do usuário.</param>
        /// <returns>Status da operação.</returns>
        /// <response code="204">Atualização realizada com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutUsuario(int id,
            [FromBody] UsuarioUpdateDTO dto)
        {
            if (id != dto.UsuarioId)
                return BadRequest("O ID do usuário não corresponde");

            await _usuariosService.UpdateUsuarioAsync(new Usuario
            {
                UsuarioId = dto.UsuarioId,
                Email = dto.Email,
                Nome = dto.Nome,
                Senha = dto.Senha
            });

            return NoContent();
        }

        /// <summary>
        /// Remove um usuário existente.
        /// </summary>
        /// <param name="id">ID do usuário a ser excluído.</param>
        /// <returns>Status da operação.</returns>
        /// <response code="204">Exclusão realizada com sucesso.</response>
        /// <response code="404">Usuário não encontrado.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _usuariosService.GetByIdAsync(id);
            if (usuario == null)
                return NotFound($"Usuário {id} não encontrado");

            await _usuariosService.DeleteUsuarioAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Atualiza os pontos de recompensa de um usuário.
        /// </summary>
        /// <param name="id">ID do usuário.</param>
        /// <param name="pontos">Quantidade de pontos a ser adicionada.</param>
        /// <returns>Status da operação.</returns>
        /// <response code="204">Pontos atualizados com sucesso.</response>
        [HttpPatch("{id}/pontos")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> AtualizarPontos(int id, [FromBody] int pontos)
        {
            await _usuariosService.UpdatePontosAsync(id, pontos);
            return NoContent();
        }
    }
}
