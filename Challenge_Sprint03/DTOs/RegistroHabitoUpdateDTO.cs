namespace Challenge_Sprint03.Models.DTOs
{
    public class RegistroHabitoUpdateDTO
    {
        public int Id { get; set; }
        public int HabitoId { get; set; }
        public string Imagem { get; set; }
        public string Observacoes { get; set; }
    }
}
