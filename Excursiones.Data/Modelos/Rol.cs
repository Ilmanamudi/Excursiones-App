using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Excursiones.Data.Modelos
{
    public class Rol
    {
        [Key]
        public int IdRol { get; set; }
        public string Descripcion { get; set; }
        public List<RolAccion> RolesAcciones { get; set; } = new List<RolAccion>();

    }
}
