using System.Security.Cryptography;
using System.Text;
using ES2_Gestao_Eventos.Database.Context;
using ES2_Gestao_Eventos.Database.Entities;
using ES2_Gestao_Eventos.Models;
using ES2_Gestao_Eventos.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ES2_Gestao_Eventos.Controllers;

 public class AutenticacaoController : Controller
    {

        private readonly GestaoEventosDBContext _context;

        public AutenticacaoController()
        {
            _context = new GestaoEventosDBContext();
        }

        // GET
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public IActionResult Login([Bind("Email", "Password")] SessaoLogin loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var passEncoded = $"\\x{ComputeSha256Hash(loginViewModel.Password)}";

            var user = _context.Users
                .Include(u => u.TipoUserNavigation)
                .FirstOrDefault(u => u.Email.Equals(loginViewModel.Email) && u.Password.Equals(passEncoded));

            if (user != null)
            {
                UserSessao.UserId = user.IdUser;
                UserSessao.Username = user.Nome;
                UserSessao.Role = user.TipoUserNavigation.Nome;

                return RedirectToAction("Login");
            }

            ViewData["HasError"] = true;

            return View(loginViewModel);
        }

        public IActionResult Registar()
        {
            TempData["MensagemRegisto"] = null;
            
            List<Tipouser> tipousers = _context.Tipousers.ToList();
            ViewBag.TipoUsers = tipousers;
            
            return View("Registar");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registar(
            [Bind("Nome", "Email", "Password", "Nascimento", "Telemovel", "IdTipoUser")] User user)
        {
            if (!UserServices.BeUniqueEmail(_context, user.Email, 0))
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

                UserSessao.UserId = user.IdUser;
                UserSessao.Username = user.Nome;
                UserSessao.Role = tipoUser?.Nome;

                _context.Add(user);
                await _context.SaveChangesAsync();
                
                TempData["MensagemRegisto"] = "Conta criada com sucesso! Faça login para acessar sua conta.";

                return View("Login");
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public IActionResult Logout()
        {
            UserSessao.Logout();
            return RedirectToAction(controllerName: "Autenticacao", actionName: "Login");
        }


        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
