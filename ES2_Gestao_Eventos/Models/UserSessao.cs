namespace ES2_Gestao_Eventos.Models;

public class UserSessao
{
    private static readonly HttpContextAccessor Accessor = new();
    
    private const string IdUserKey = "IdUser";
    private const string UserNameKey = "Username";
    private const string RoleKey = "Role";
    
    public static bool HasSession()
    {
        var id = UserId;
        if (id != null)
        {
            return true;
        }

        return false;
    }
        
    public static void Logout()
    {
        UserId = null;
        Role = null;
        Username = null;
    }
    
    public static string? Username
    {
        get => Accessor.HttpContext?.Session.GetString(UserNameKey);
        set
        {
            if (value != null)
            {
                Accessor.HttpContext?.Session.SetString(UserNameKey, value);
            }
            else
            {
                Accessor.HttpContext?.Session.Remove(UserNameKey);
            }
        }
    }

    public static int? UserId
    {
        get => Accessor.HttpContext?.Session.GetInt32(IdUserKey);
        set
        {
            if (value != null)
            {
                Accessor.HttpContext?.Session.SetInt32(IdUserKey, (int)value);
            }
            else
            {
                Accessor.HttpContext?.Session.Remove(IdUserKey);
            }
        }
    }

    public static string? Role
    {
        get => Accessor.HttpContext?.Session.GetString(RoleKey);
        set
        {
            if (value != null)
            {
                Accessor.HttpContext?.Session.SetString(RoleKey, (string)value);
            }
            else
            {
                Accessor.HttpContext?.Session.Remove(RoleKey);
            }
        }
    }
}