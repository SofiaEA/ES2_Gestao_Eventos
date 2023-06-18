using ES2_Gestao_Eventos.Database.Context;
using ES2_Gestao_Eventos.Middleware;
using Microsoft.EntityFrameworkCore;

namespace ES2_Gestao_Eventos;

public class Startup
{
    public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure services such as database, authentication, etc.

            services.AddDbContext<GestaoEventosDBContext>(options =>
            {
                options.UseNpgsql("Host=localhost;Database=GestaoEventos;Username=postgres;Password=sofiaam;SearchPath=public;");
            });
    
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
   
            services.AddControllersWithViews();
            
            
            services.AddRazorPages();
            services.AddHttpContextAccessor();
            services.AddDistributedMemoryCache();
            services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(60); });
            
            // Add framework services.
            services.AddMvc();
        }
    
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Configure production error handling
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
    
            app.UseHttpsRedirection();
            app.UseStaticFiles();
    
            app.UseRouting();
            app.UseAuthorization();
            app.UseSession();
    
            // Configure middleware such as authentication, authorization, etc.
            
            //Custom Middlewares
            app.UseMiddleware<AutenticacaoMidlleware>();
    
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Autenticacao}/{action=Login}/{id?}");
                endpoints.MapRazorPages();
            });
        }
}