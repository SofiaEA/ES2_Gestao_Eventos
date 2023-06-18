using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ES2_Gestao_Eventos.Database.Entities
{
    [Table("eventos")]
    [Index("IdEvento", Name = "eventos_idevento_uindex", IsUnique = true)]
    public partial class Evento
    {
        [Key]
        [Column("idEvento")]
        public int IdEvento { get; set; }

        [Column("nome")]
        [StringLength(255)]
        public string Nome { get; set; } = null!;

        [Column("local")]
        [StringLength(100)]
        public string Local { get; set; } = null!;

        [Column("descricao")]
        [StringLength(100)]
        public string? Descricao { get; set; }

        [Column("capacidademax")]
        public int Capacidademax { get; set; }

        [Column("data")]
        public DateTime Data { get; set; }

        [Column("idCategoria")]
        public int? IdCategoria { get; set; }

        [Column("idUser")]
        public int? IdUser { get; set; }
        
        [Column("procurar")]
        public string? procurar { get; set; }

        [InverseProperty("IdEventoNavigation")]
        public virtual ICollection<Bilhete> Bilhetes { get; set; } = new List<Bilhete>();

        [ForeignKey(nameof(IdCategoria))]
        public virtual Categoria? IdCategoriaNavigation { get; set; }

        [ForeignKey(nameof(IdUser))]
        public virtual User? IdUserNavigation { get; set; }
        
    }
}