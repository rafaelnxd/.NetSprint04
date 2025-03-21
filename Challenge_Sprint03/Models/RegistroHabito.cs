using Challenge_Sprint03.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class RegistroHabito
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int HabitoId { get; set; }
    public DateTime Data { get; set; }
    public string Imagem { get; set; }
    public string Observacoes { get; set; }
    // Propriedade de navegação para o hábito – certifique-se de que NÃO está mapeada como string!
    public Habito Habito { get; set; }
}
