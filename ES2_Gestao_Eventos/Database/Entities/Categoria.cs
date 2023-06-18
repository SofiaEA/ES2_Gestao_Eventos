using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ES2_Gestao_Eventos.Database.Entities;

[Table("categorias")]
[Index("IdCategoria", Name = "categorias_idcategoria_uindex", IsUnique = true)]
public partial class Categoria
{
    [Key]
    [Column("idCategoria")]
    public int IdCategoria { get; set; }

    [Column("nome")]
    [StringLength(255)]
    public string Nome { get; set; }

    [InverseProperty("IdCategoriaNavigation")]
    public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();
}
