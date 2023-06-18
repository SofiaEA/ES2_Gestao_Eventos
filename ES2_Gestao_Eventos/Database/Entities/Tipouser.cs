using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ES2_Gestao_Eventos.Database.Entities
{
    [Table("tipouser")]
    [Index("IdTipoUser", Name = "tipouser_idtipouser_uindex", IsUnique = true)]
    public partial class Tipouser
    {
        [Key]
        [Column("idTipoUser")]
        public int IdTipoUser { get; set; }

        [Column("nome")]
        [StringLength(100)]
        public string? Nome { get; set; }

        [InverseProperty("TipoUserNavigation")]
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}