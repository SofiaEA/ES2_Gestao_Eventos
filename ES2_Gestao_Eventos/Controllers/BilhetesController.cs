using ES2_Gestao_Eventos.Database.Context;
using ES2_Gestao_Eventos.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ES2_Gestao_Eventos.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ES2_Gestao_Eventos.Controllers
{
    public class BilhetesController : Controller
    {
        private readonly GestaoEventosDBContext _context;

        public BilhetesController(GestaoEventosDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ListarBilhetes()
        {
            var userId = UserSessao.UserId; 

            var bilhetes = await _context.Bilhetes
                .Include(b => b.IdEventoNavigation)
                .Include(tb => tb.IdTipoBilhetesNavigation)
                .Where(b => b.IdEventoNavigation.IdUser == userId)
                .ToListAsync();

            return View(bilhetes);
        }

        public IActionResult CriarBilhetes()
        {
            
               var currentUser = UserSessao.UserId;
               
            var eventos = _context.Eventos
                .Where(e => e.IdUserNavigation.IdUser == currentUser)
                .ToList();
            
            var tiposBilhete = _context.Tipobilhetes.ToList();
            
            var eventosSelectList = new List<SelectListItem>();
            foreach (var evento in eventos)
            {
                eventosSelectList.Add(new SelectListItem
                {
                    Value = evento.IdEvento.ToString(),
                    Text = evento.Nome
                });
            }
            
            var tiposBilheteSelectList = new List<SelectListItem>();
            foreach (var tipoBilhete in tiposBilhete)
            {
                tiposBilheteSelectList.Add(new SelectListItem
                {
                    Value = tipoBilhete.IdTipoBilhete.ToString(),
                    Text = tipoBilhete.Nome
                });
            }
            
            ViewBag.Eventos = eventosSelectList;
            ViewBag.TiposBilhete = tiposBilheteSelectList;

            return View();
        }


        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CriarBilhetes(
            [Bind("IdEvento", "IdTipoBilhetes", "Precototal", "Numbilhetes", "bilhetescomprados")] Bilhete bilhetes)
        {
            if (!ModelState.IsValid)
            {
                return View(bilhetes);
            }

            try
            {
                bilhetes.bilhetesComprados = 0;
                
                var evento = _context.Eventos
                    .FirstOrDefault(tb => tb.IdEvento.Equals(bilhetes.IdEvento));
                
                var tiposBilhete = _context.Tipobilhetes
                    .FirstOrDefault(tb => tb.IdTipoBilhete.Equals(bilhetes.IdTipoBilhetes));

                if (evento != null)
                {
                    bilhetes.IdEventoNavigation = evento;
                    bilhetes.IdTipoBilhetesNavigation = tiposBilhete;

                    _context.Add(bilhetes);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("ListarBilhetes");
                }
                
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
    
}