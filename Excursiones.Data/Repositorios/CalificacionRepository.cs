using Excursiones.Data.Db;
using Excursiones.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Excursiones.Data.Repositorios
{
    public class CalificacionRepository : ICalificacionRepository
    {
        private readonly ExcursionesContext _db; 

        public CalificacionRepository(ExcursionesContext db)
        {
            _db = db;
        }

        public void Insertar(Calificacion calificacion)
        {
            _db.Calificaciones.Add(calificacion);
            _db.SaveChanges();
        }

        public bool ExisteCalificacion(int idReserva)
        {
            return _db.Calificaciones.Any(c => c.IdReserva == idReserva);
        }

        public (double Promedio, int TotalResenas) ObtenerEstadisticasPorDestino(string nombreDestino)
        {
            
            var calificaciones = _db.Calificaciones
                .Where(c => c.Reserva.Excursion.NombreExcursion.ToLower() == nombreDestino.ToLower())
                .ToList();

            if (!calificaciones.Any())
            {
                return (0, 0);
            }

            double promedio = calificaciones.Average(c => c.Puntuacion);
            int total = calificaciones.Count;

            return (promedio, total);
        }
    }
}