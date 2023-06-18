using ES2_Gestao_Eventos.Database.Context;
using ES2_Gestao_Eventos.Database.Entities;
using ES2_Gestao_Eventos.Models;
using ES2_Gestao_Eventos.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ES2_Gestao_Eventos.Controllers;

public class PerfilController : Controller
{

    private readonly GestaoEventosDBContext _context;

    public PerfilController()
    {
        _context = new GestaoEventosDBContext();
    }


    public async Task<IActionResult> VerPerfil()
    {
        var currentUser = UserSessao.UserId;

        var user = await _context.Users
            .Include(u =>
                u.TipoUserNavigation) // 
            .FirstOrDefaultAsync(u => u.IdUser == currentUser);

        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }
    
    public async Task<IActionResult> EditarPerfil(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var currentUser = await _context.Users
            .FindAsync(id);
        
        if (currentUser == null)
        {
            return NotFound();
        }
        return View(currentUser);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditarPerfil(int id, [Bind("IdUser", "Nome", "Email", "Password", "Telemovel", "Nascimento", "IdTipoUser")] User user)
    {

        if (id != user.IdUser)
        {
            return NotFound();
        }
        
        if (UtilServices.DateGratherThanToday(user.Nascimento))
        {
            ViewData["dateGreather"] = true;
            return View();
        }
        
        user.Nascimento = DateTime.SpecifyKind(user.Nascimento, DateTimeKind.Utc);

        if (ModelState.IsValid)
        {

            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return RedirectToAction(nameof(VerPerfil));
        }

        return View(user);
    }


}