using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ES2_Gestao_Eventos.Database.Entities
{
    [Table("tipobilhetes")]
    [Index("IdTipoBilhete", Name = "tipobilhetes_idtipobilhetes_uindex", IsUnique = true)]
    public partial class Tipobilhete
    {
        [Key] [Column("idTipoBilhete")] public int IdTipoBilhete { get; set; }

        [Column("nome")] [StringLength(255)] public string? Nome { get; set; } = null!;


        [InverseProperty("IdTipoBilhetesNavigation")]
        public virtual ICollection<Bilhete> Bilhetes { get; set; } = new List<Bilhete>();
    }
}
