using System.ComponentModel.DataAnnotations;

namespace ES2_Gestao_Eventos.Models;

public class TipoBilheteViewModel
{
    [Required]
    public int? Nome { get; set; }
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "A quantidade tem de seer positiva!!")]
    public int numBilhetes { get; set; }
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Preço tem de ser maior que 0!!")]
    public double preco { get; set; }
}