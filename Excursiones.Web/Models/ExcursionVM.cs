using System.ComponentModel.DataAnnotations;

namespace Excursiones.Web.Models
{
    public class ExcursionVM
    {
        public int IdExcursion { get; set; }

        [Required(ErrorMessage = "El nombre de la excursión es obligatorio.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "El nombre debe tener al menos 5 caracteres.")]
        public string NombreExcursion { get; set; }

        [Required(ErrorMessage = "La fecha de salida es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaExcursion { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El cupo es obligatorio.")]
        public int Cupo { get; set; }

        [Required(ErrorMessage = "Debes seleccionar un estado.")]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor, selecciona un estado válido.")]
        public int IdEstado { get; set; }

        public string? Descripcion { get; set; }
        public int DuracionDias { get; set; }

        public string? NombreEstado { get; set; }
        public string? ColorEstado { get; set; }

        [Display(Name = "Imagen de la Excursión")]
        public IFormFile? ImagenFile { get; set; }

        public string? ImagenUrl { get; set; }

    }
}
