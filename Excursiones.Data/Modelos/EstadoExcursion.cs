using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Excursiones.Data.Modelos
{
    public class EstadoExcursion
    {

        [Key]
        public int IdEstado { get; set; }

        [Required]
        [MaxLength(50)]
        public string NombreEstado { get; set; } 

    }
}
