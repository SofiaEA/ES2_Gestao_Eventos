using System.ComponentModel.DataAnnotations.Schema;
using ES2_Gestao_Eventos.Database.Entities;

namespace ES2_Gestao_Eventos.Models;

public class EventosViewModel
{
    public int IdEvento { get; set; }
    public int? IdUser { get; set; }
    public string Nome { get; set; }
    
    public string Local { get; set; }
    
    public string Descricao { get; set; }
    
    public int CapacidadeMax { get; set; }
    
    public int BilhetesDisp { get; set; }
    
    public DateTime Data { get; set; }
    
    [Column("idCategoria")]
    public int? idCategoria { get; set; }
    
    [Column("IdBilhetes")]
    public int? IdBilhetes { get; set; }
    
    public BilheteViewModel Bilhete { get; set; }
    
    public List<BilheteViewModel> Bilhetes { get; set; }
    
    public Categoria IdCategoriaNavigation { get; set; }
    
    public virtual Bilhete IdBilheteNavigation { get; set; }
}