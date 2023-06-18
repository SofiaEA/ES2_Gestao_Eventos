namespace ES2_Gestao_Eventos.Services;

public class UtilServices
{
    public static bool DateGratherThanToday(DateTime? date)
    {
        DateTime localDate = DateTime.Now.Date;
            
        return localDate < date;
    }
}