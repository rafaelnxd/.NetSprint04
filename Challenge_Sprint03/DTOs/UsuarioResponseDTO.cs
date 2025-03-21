using System;

namespace Challenge_Sprint03.Models.DTOs
{
    public class UsuarioResponseDTO
    {
        public int UsuarioId { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public DateTime DataCadastro { get; set; }
        public int PontosRecompensa { get; set; }
    }
}
