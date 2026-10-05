using Excursiones.Data.Db;
using Excursiones.Data.Modelos;
using Microsoft.EntityFrameworkCore;

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

    }

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
                // Log the exception (ex) here if needed
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

    }
}
