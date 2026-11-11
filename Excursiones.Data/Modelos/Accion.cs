using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Excursiones.Data.Modelos
{
    public class Accion
    {
        [Key]
        public int IdAccion { get; set; }
        public string Descripcion { get; set; }
    }
}
