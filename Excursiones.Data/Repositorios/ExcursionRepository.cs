using Excursiones.Data.Db;
using Excursiones.Data.Modelos;
using Microsoft.EntityFrameworkCore;

namespace Excursiones.Data.Repositorios
{
    public class ExcursionRepository : IExcursionRepository
    {
        private ExcursionesContext _db;

        public ExcursionRepository(ExcursionesContext context)
        {
            _db = context;
        }

        public IEnumerable<EstadoExcursion> ObtenerEstadosExcursion()
        {
            return _db.EstadosExcursion.ToList();
        }
        public bool AgregarExcursion(Excursion excursion)
        {
            try
            {
                _db.Excursiones.Add(excursion);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
             
                return false;
            }
        }

        public List<Excursion> ObtenerExcursiones()
        {
            return _db.Excursiones.ToList();
        }

        public Excursion ObtenerExcursionPorId(int IdExcursion)
        {
            Excursion excursion = _db.Excursiones.Find(IdExcursion);

            return excursion;
        }

        public void ActualizarExcursion(Excursion excursion)
        {
            _db.Excursiones.Update(excursion);
            _db.SaveChanges();
        }

        public bool BorrarExcursion(int IdExcursion)
        {
            try
            {
                Excursion excursion = _db.Excursiones.Find(IdExcursion);
                if (excursion != null)
                {
                    _db.Excursiones.Remove(excursion);
                    _db.SaveChanges();
                    return true;
                }
                return false; 
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }

        public void ActualizarEstadosAutomaticamente()
        {
            
            var excursionesPasadas = _db.Excursiones
                .Where(e => e.IdEstado == 1 && e.FechaExcursion.Date < DateTime.Now.Date)
                .ToList();

            if (excursionesPasadas.Any())
            {
                foreach (var excursion in excursionesPasadas)
                {
                    excursion.IdEstado = 2; 
                }

                
                _db.SaveChanges();
            }
        }

    }
}
