using System.ComponentModel.DataAnnotations;
using ES2_Gestao_Eventos.Database.Entities;

namespace ES2_Gestao_Eventos.Models
{
    public class BilheteViewModel
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Deve ser um número positivo!")]
        public double PrecoTotal { get; set; }
        
        // public string NomeEvento { get; set; }
        public Tipobilhete Bilhete { get; set; }
        
        public Evento Evento { get; set; }
    }
}