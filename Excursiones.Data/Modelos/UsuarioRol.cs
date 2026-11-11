using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Excursiones.Data.Modelos
{
    public class UsuarioRol
    {
        [Key]
        public int IdUsuarioRol { get; set; }

        public int IdUsuario { get; set; }
        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; } = null!;

        public int IdRol { get; set; }
        [ForeignKey("IdRol")]
        public Rol Rol { get; set; } = null!;
    }
}
