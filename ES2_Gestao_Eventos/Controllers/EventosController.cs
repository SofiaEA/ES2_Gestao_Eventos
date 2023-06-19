using ES2_Gestao_Eventos.Database.Context;
using ES2_Gestao_Eventos.Database.Entities;
using ES2_Gestao_Eventos.Models;
using ES2_Gestao_Eventos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ES2_Gestao_Eventos.Controllers;

public class EventosController : Controller
{
    private readonly GestaoEventosDBContext _context;

    public EventosController()
    {
        _context = new GestaoEventosDBContext();
    }

    public async Task<IActionResult> ListarEventos()
    {
        
        var currentUser = UserSessao.UserId;
        
        var eventos = await _context.Eventos
            .Include(e => e.IdCategoriaNavigation)
            .Where(e => e.IdUserNavigation.IdUser == currentUser)
            .ToListAsync();

        return View(eventos);
    }
    
    public async Task<IActionResult> Relatório(int id)
    {
        var userId = UserSessao.UserId;

        var evento = await _context.Eventos
            .Include(e => e.IdCategoriaNavigation)
            .Include(v => v.Bilhetes)
            .ThenInclude(t => t.IdTipoBilhetesNavigation)
            .FirstOrDefaultAsync(e => e.IdEvento == id && e.IdUserNavigation.IdUser == userId);

        if (evento == null)
        {
            return NotFound();
        }

        return View(evento);
    }

    public async Task<IActionResult> ListarEventosTodos()
    {
        
        var ev = await _context.Eventos
            .Include(e => e.IdCategoriaNavigation)
            .ToListAsync();

        return View(ev);
    }
    
    public async Task<IActionResult> ListarEventosManager()
    {
        
        var evento = await _context.Eventos
            .Include(e => e.IdUserNavigation)
            .Include(e => e.IdCategoriaNavigation)
            .ToListAsync();

        return View(evento);
    }
    

    public async Task<IActionResult> InscreverEvento(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
           
        var evento = await _context.Eventos
            .Include(e => e.Bilhetes)
            .ThenInclude(b => b.IdTipoBilhetesNavigation)
            .FirstOrDefaultAsync(e => e.IdEvento == id);
        
        if (evento == null)
        {
            return NotFound();
        }
        var bilhetes = evento.Bilhetes.ToList();
        return View(bilhetes);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> InscreverEvento(int id,
        [Bind("IdEvento", "IdTipoBilhete", "Quantidade")] Bilhete bib)
    {
        if (id != bib.IdEvento)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(bib);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return RedirectToAction(nameof(ListarEventosTodos));
        }

        return View((IEnumerable<Bilhete>)bib);
    }

    public async Task<IActionResult> ComprarBilhete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var bilhete = await _context.Bilhetes
            .Include(b => b.IdEventoNavigation)
            .Include(b => b.IdTipoBilhetesNavigation)
            .FirstOrDefaultAsync(b => b.IdBilhete == id);

        if (bilhete == null)
        {
            return NotFound();
        }

        return View(bilhete);
    }



    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmarCompra(int id, int quantidade)
    {
        var bilhete = await _context.Bilhetes.FindAsync(id);

        if (bilhete == null)
        {
            return NotFound();
        }

        if (bilhete.bilhetesdisp < quantidade)
        {
            TempData["MensagemCompra"] = "Não há bilhetes suficientes disponíveis.";
            return RedirectToAction(nameof(ComprarBilhete), new { id = bilhete.IdBilhete });
        }

        bilhete.bilhetesdisp -= quantidade;
        bilhete.bilhetesComprados += quantidade;

        try
        {
            _context.Update(bilhete);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            TempData["MensagemCompra"] = "Ocorreu um erro ao processar a compra.";
            return RedirectToAction(nameof(ComprarBilhete), new { id = bilhete.IdBilhete });
        }

        TempData["MensagemCompra"] = $"Compra efetuada com sucesso! Quantidade: {quantidade}";

        return RedirectToAction(nameof(ListarEventosTodos));
    }

    public IActionResult CriarEvento()
    {
        var currentUser = UserSessao.UserId;
        
        List<Categoria> categoria = _context.Categorias.ToList();
        ViewBag.Categorias = categoria;

        return View();
        
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
       public async Task<IActionResult> CriarEvento(
            [Bind("Nome", "Local", "Descricao", "Capacidademax", "Data", "IdCategoria","IdUser")] Evento eventos)
        {
            if (!ModelState.IsValid)
            {
                return View(eventos);
            }
            
            
            eventos.IdUser = UserSessao.UserId;
            
            eventos.Data = DateTime.SpecifyKind((DateTime)eventos.Data, DateTimeKind.Utc);
            
            try
            {
                var cat = _context.Categorias
                    .FirstOrDefault(tb => tb.IdCategoria.Equals(eventos.IdCategoria));
                

                _context.Add(eventos);
                await _context.SaveChangesAsync();

                return RedirectToAction(controllerName: "Eventos", actionName: "ListarEventos");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
       

       public async Task<IActionResult> EditarEvento(int? id)
       {
           if (id == null)
           {
               return NotFound();
           }
           
           var eve = await _context.Eventos
               .Include(v => v.Bilhetes)
               .ThenInclude(t => t.IdTipoBilhetesNavigation)
               .FirstOrDefaultAsync(e => e.IdEvento == id);
        
           if (eve == null)
           {
               return NotFound();
           }
           return View(eve);
       }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarEvento(int id,
            [Bind("IdEvento", "Nome", "Local", "Descricao", "Capacidademax", "Data", "IdCategoria","IdUser")] Evento eventos)
        {
            if (id != eventos.IdEvento)
            {
                return NotFound();
            }

            eventos.Data = DateTime.SpecifyKind(eventos.Data, DateTimeKind.Utc);
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventos);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                return RedirectToAction(nameof(ListarEventos));
            }

            return View(eventos);
        }
        
        // GET: Event/Search
        public IActionResult ProcurarEvento()
        {
            var model = new ProcurarEventosViewModel();
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProcurarEvento(ProcurarEventosViewModel model)
        {
            if (ModelState.IsValid)
            {
            
                string searchQuery = model.SearchQuery.ToLower();

                var procurar = _context.Eventos.ToList()
                    .Where(e => e.Nome.IndexOf(searchQuery, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                e.Local.IndexOf(searchQuery, StringComparison.OrdinalIgnoreCase) >= 0 ||
                               e.Data.ToString().Contains(searchQuery))
                    .ToList();
            
                model.procurar = procurar;
            
            }

            return View(model);
        }
        
    }