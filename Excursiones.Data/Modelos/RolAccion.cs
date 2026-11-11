using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Excursiones.Data.Modelos
{
    public class RolAccion
    {
        [Key]
        public int IdRolAccion { get; set; }
        public int IdRol { get; set; }
        [ForeignKey("IdRol")]
        public Rol Rol { get; set; } = null!;
        public int IdAccion { get; set; }
        [ForeignKey("IdAccion")]
        public Accion Accion { get; set; } = null!;
    }
}
