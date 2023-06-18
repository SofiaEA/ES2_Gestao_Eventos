using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ES2_Gestao_Eventos.Database.Entities
{
    [Table("users")]
    [Index("IdUser", Name = "user_iduser_uindex", IsUnique = true)]
    [Index("Email", Name = "users_email_key", IsUnique = true)]
    public partial class User
    {
        [Key]
        [Column("idUser")]
        public int IdUser { get; set; }

        [Column("nome")]
        [StringLength(255)]
        public string Nome { get; set; } = null!;

        [Column("email")]
        [StringLength(100)]
        public string Email { get; set; } = null!;

        [Column("password")]
        [StringLength(100)]
        public string Password { get; set; } = null!;

        [Column("telemovel")]
        public int? Telemovel { get; set; }

        [Column("nascimento")]
        public DateTime Nascimento { get; set; }

        [Column("idTipoUser")]
        public int? IdTipoUser { get; set; }

        [InverseProperty("IdUserNavigation")]
        public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();

        [ForeignKey(nameof(IdTipoUser))]
        public virtual Tipouser? TipoUserNavigation { get; set; }
    }
}