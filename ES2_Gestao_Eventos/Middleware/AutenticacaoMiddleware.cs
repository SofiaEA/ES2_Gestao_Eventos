using ES2_Gestao_Eventos.Models;

namespace ES2_Gestao_Eventos.Middleware;

public class AutenticacaoMidlleware
{
    private readonly RequestDelegate _next;

    public AutenticacaoMidlleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext httpContext)
    {
        var path = httpContext.Request.Path;


        if (path.HasValue && (path.Value.StartsWith("/Autenticacao") || path.Value == "/"))
        {
            if (UserSessao.HasSession())
            {
                httpContext.Response.Redirect("/Home/Index");
            }
        }
        else
        {
            if (!UserSessao.HasSession())
            {
                httpContext.Response.Redirect("/Autenticacao/Login");
            }
        }


        return _next(httpContext);
    }
}