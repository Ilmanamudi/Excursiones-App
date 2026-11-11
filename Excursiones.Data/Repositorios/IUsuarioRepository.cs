using Excursiones.Data.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Excursiones.Data.Repositorios
{
    public interface IUsuarioRepository
    {
        Usuario ObtenerOCrearUsuario(string email, string nombre);
        List<String> ObtenerAcciones(string email);

    }
}
