using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Excursiones.Data.Modelos
{
    public class Calificacion
    {
        [Key]
        public int IdCalificacion { get; set; }

        [Required]
        public int IdReserva { get; set; }

        [ForeignKey("IdReserva")]
        public Reserva? Reserva { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "La puntuación debe ser entre 1 y 5 estrellas.")]
        public int Puntuacion { get; set; }

        [MaxLength(500, ErrorMessage = "El comentario no puede superar los 500 caracteres.")]
        public string? Comentario { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}