using System.ComponentModel.DataAnnotations;
using ES2_Gestao_Eventos.Database.Entities;

namespace ES2_Gestao_Eventos.Models;

public class ProcurarEventosViewModel
{
    [Required]
        public string SearchQuery { get; set; }
    
        public List<Evento> procurar { get; set; }
    
        public ProcurarEventosViewModel()
        {
            procurar = new List<Evento>();
        }
}