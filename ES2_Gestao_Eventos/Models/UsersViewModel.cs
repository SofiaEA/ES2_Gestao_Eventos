using ES2_Gestao_Eventos.Database.Entities;

namespace ES2_Gestao_Eventos.Models;

public class UsersViewModel
{
    public string Nome { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public string Telemovel { get; set; }
    
    public int Nascimento { get; set; }
    
    public Tipouser User { get; set; }
    
}