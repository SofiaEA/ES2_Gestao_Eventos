using ES2_Gestao_Eventos;
using ES2_Gestao_Eventos.Database.Context;


namespace Gestao_Eventos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //initialize Data
            CreateAdminIfNotExists();

            CreateHostBuilder(args).Build().Run();
        }
        
        private static void CreateAdminIfNotExists()
        {
            try
            {
                GestaoEventosDBContext context = new GestaoEventosDBContext();
                DBinitial.Initialize(context);
            }
            catch (Exception ex)
            {
            }
        }
        
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
;