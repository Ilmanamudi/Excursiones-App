using Excursiones.Data.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Excursiones.Data.Repositorios
{
    public interface ICalificacionRepository
    {
        void Insertar(Calificacion calificacion);
        bool ExisteCalificacion(int idReserva);

        (double Promedio, int TotalResenas) ObtenerEstadisticasPorDestino(string nombreDestino);
    }
}
