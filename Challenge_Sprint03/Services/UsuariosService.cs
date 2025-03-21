using Challenge_Sprint03.Models;
using Challenge_Sprint03.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge_Sprint03.Services
{
    public class UsuariosService
    {
        private readonly IRepository<Usuario> _usuarioRepository;

        public UsuariosService(IRepository<Usuario> usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _usuarioRepository.GetAllAsync();
        }

        public async Task<Usuario> GetByIdAsync(int id)
        {
            return await _usuarioRepository.GetByIdAsync(id);
        }

        public async Task<Usuario> CreateUsuarioAsync(Usuario usuario)
        {
            // Verifica se o email já existe
            var usuariosExistentes = await _usuarioRepository.FindAsync(u => u.Email == usuario.Email);
            if (usuariosExistentes.Any())
                throw new Exception("Email já cadastrado");

            usuario.DataCadastro = DateTime.Now;
            usuario.PontosRecompensa = 0;

            await _usuarioRepository.AddAsync(usuario);
            return usuario;
        }

        public async Task UpdateUsuarioAsync(Usuario usuario)
        {
            var usuarioExistente = await _usuarioRepository.GetByIdAsync(usuario.UsuarioId);
            if (usuarioExistente == null)
                throw new Exception("Usuário não encontrado");

            // Atualiza os campos necessários
            usuarioExistente.Email = usuario.Email;
            usuarioExistente.Nome = usuario.Nome;
            usuarioExistente.Senha = usuario.Senha;

            await _usuarioRepository.UpdateAsync(usuarioExistente);
        }

        public async Task DeleteUsuarioAsync(int id)
        {
            await _usuarioRepository.DeleteAsync(id);
        }

        public async Task UpdatePontosAsync(int id, int pontos)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
                throw new Exception("Usuário não encontrado");

            usuario.PontosRecompensa += pontos;
            await _usuarioRepository.UpdateAsync(usuario);
        }
    }
}
