using ES2_Gestao_Eventos.Controllers;
using ES2_Gestao_Eventos.Database.Context;
using ES2_Gestao_Eventos.Database.Entities;

namespace ES2_Gestao_Eventos;

public class DBinitial
{
    public static void Initialize(GestaoEventosDBContext context)
    {
        context.Database.EnsureCreated();
            
        // Look for any user.
        if (context.Users.Any())
        {
            return; // DB has been seeded
        }
        var passEncoded = $"\\x{AutenticacaoController.ComputeSha256Hash("1234567")}";

        //create Default Admin
        DateTime dataNascimento = DateTime.Parse("09-09-1999");
        var admin = new User
        {
            Nome = "admin",
            Email = "admin@ipvc.pt",
            Password = passEncoded,
            Telemovel = 957395444,
            Nascimento = dataNascimento,
            IdTipoUser = 1
                
        };
        context.Users.Add(admin);
        context.SaveChanges();
    }
}