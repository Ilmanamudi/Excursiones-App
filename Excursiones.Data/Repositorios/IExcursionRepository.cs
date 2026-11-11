using Excursiones.Data.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Excursiones.Data.Repositorios
{
    public interface IExcursionRepository
    {
        void ActualizarExcursion(Excursion excursion);

        bool AgregarExcursion(Excursion excursion);

        bool BorrarExcursion(int IdExcursion);

        Excursion ObtenerExcursionPorId(int IdExcursion);

        List<Excursion> ObtenerExcursiones();

        IEnumerable<EstadoExcursion> ObtenerEstadosExcursion();
        void ActualizarEstadosAutomaticamente();


    }
}
