using System.ComponentModel.DataAnnotations;

namespace ES2_Gestao_Eventos.Models;


    public class SessaoLogin
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    
        [Required]
        [DataType(DataType.Password)]
        [MinLength(7)]
        public string Password { get; set; }
    }