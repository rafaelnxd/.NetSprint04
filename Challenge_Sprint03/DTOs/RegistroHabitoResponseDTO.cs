using System;

namespace Challenge_Sprint03.Models.DTOs
{
    public class RegistroHabitoResponseDTO
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string Observacoes { get; set; }
        public string HabitoDescricao { get; set; }
    }
}
