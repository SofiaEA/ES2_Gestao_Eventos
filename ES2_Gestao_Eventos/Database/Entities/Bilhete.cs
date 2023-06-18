using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ES2_Gestao_Eventos.Database.Entities
{
    [Table("bilhetes")]
    [Index("IdBilhete", Name = "bilhetes_idbilhete_uindex", IsUnique = true)]
    public partial class Bilhete
    {
        [Key]
        [Column("idBilhete")]
        public int IdBilhete { get; set; }

        [Column("precototal")]
        public double Precototal { get; set; }
        
        [Column("bilhetesdisp")]
        public int bilhetesdisp { get; set; }

        [Column("idTipoBilhetes")]
        public int IdTipoBilhetes { get; set; }

        [Column("idEvento")]
        public int IdEvento { get; set; }
        
        [Column("numbilhetes")]
        public int Numbilhetes { get; set; }

        [ForeignKey(nameof(IdEvento))]
        public virtual Evento? IdEventoNavigation { get; set; }

        [ForeignKey(nameof(IdTipoBilhetes))]
        public virtual Tipobilhete? IdTipoBilhetesNavigation { get; set; }
    }
}