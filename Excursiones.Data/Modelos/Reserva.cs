using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Excursiones.Data.Modelos
{
    public class Reserva
    {
        [Key]
        public int IdReserva { get; set; }

        [Required]
        public int IdExcursion { get; set; }

        [Required]
        public int IdUsuario { get; set; }

        [Required]
        public int CantidadLugares { get; set; }

        public DateTime FechaReserva { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAbonado { get; set; }
        [ForeignKey("IdExcursion")]
        public virtual Excursion Excursion { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; }
    }
}