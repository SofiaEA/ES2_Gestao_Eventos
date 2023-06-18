using System.Security.Cryptography;
using System.Text;
using ES2_Gestao_Eventos.Database.Context;
using ES2_Gestao_Eventos.Database.Entities;
using ES2_Gestao_Eventos.Models;
using ES2_Gestao_Eventos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ES2_Gestao_Eventos.Controllers;

public class UtilizadoresController : Controller
{
    private readonly GestaoEventosDBContext _context;

    public UtilizadoresController()
    {
        _context = new GestaoEventosDBContext();
    }

    public async Task<IActionResult> ListarUsers()
    {
        return View(await _context.Users
            .Include(u => u.TipoUserNavigation).ToListAsync());
    }
    
    public IActionResult CriarUsers()
    {
        List<Tipouser> tipousers = _context.Tipousers.ToList();
        ViewBag.TipoUsers = tipousers;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CriarUsers(
        [Bind("Nome", "Email", "Password", "Telemovel", "Nascimento", "IdTipoUser")] User user)
    {

        if(!UserServices.BeUniqueEmail(_context, user.Email, 0))
        {
            ViewData["emailUniqueError"] = true;
            return View();
        }

        if (UtilServices.DateGratherThanToday(user.Nascimento))
        {
            ViewData["birthDateError"] = true;
            return View();
        }

        if (!ModelState.IsValid)
        {
            return View(user);
        }
        

        user.Nascimento = DateTime.SpecifyKind(user.Nascimento, DateTimeKind.Utc);

        var passEncoded = $"\\x{ComputeSha256Hash(user.Password)}";
        user.Password = passEncoded;

        try
        {

            var tipoUser = _context.Tipousers
                .FirstOrDefault(u => u.IdTipoUser.Equals(user.IdTipoUser));

            _context.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(controllerName: "Utilizadores", actionName: "ListarUsers");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }      
    public static string ComputeSha256Hash(string rawData)
             {
                 using (SHA256 sha256Hash = SHA256.Create())
                 {
                     byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                     
                     StringBuilder builder = new StringBuilder();
                     for (int i = 0; i < bytes.Length; i++)
                     {
                         builder.Append(bytes[i].ToString("x2"));
                     }
     
                     return builder.ToString();
                 }
             }
    
    
    public async Task<IActionResult> EliminarUsers(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var eli = await _context.Users.Include(u => u.TipoUserNavigation)
            .FirstOrDefaultAsync(e => e.IdUser == id);

        if (eli == null)
        {
            return NotFound();
        }

        return View(eli);
    }
    
    [HttpPost, ActionName("EliminarUsers")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmDelete(int id)
    {
        var eli = await _context.Users.FindAsync(id);
        _context.Users.Remove(eli);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(ListarUsers));
    }
}
