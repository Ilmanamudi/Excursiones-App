using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Excursiones.Data.Modelos
{
    public class Excursion
    {
        [Key]
        public int IdExcursion { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreExcursion { get; set; }

        [Column(TypeName = "Date")]
        public DateTime FechaExcursion { get; set; }

        [Column(TypeName = "decimal(18,2)")] 
        public decimal Precio { get; set; }
        public int Cupo { get; set; }

        [MaxLength(500)] 
        public string? Descripcion { get; set; } 

        public int DuracionDias { get; set; }

        [Column(TypeName = "bit")]
        public bool Borrado { get; set; }

        [Required]
        public int IdEstado { get; set; } 

        [ForeignKey("IdEstado")]
        public virtual EstadoExcursion Estado { get; set; } 
    }

}
