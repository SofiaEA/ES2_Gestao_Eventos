using ES2_Gestao_Eventos.Database.Context;

namespace ES2_Gestao_Eventos.Services;

public class UserServices
{
    public static bool BeUniqueEmail(GestaoEventosDBContext context,string email, int id)
    {
        return context.Users.FirstOrDefault(x =>x.Email == email && x.IdUser != id) == null;
    }
}
