using Excursiones.Data.Db;
using Excursiones.Data.Modelos;
using Excursiones.Data.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace Excursiones.Data.Repositorios
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly ExcursionesContext _db;

        public ReservaRepository(ExcursionesContext db)
        {
            _db = db;
        }

       

        public void Insertar(Reserva reserva)
        {
            _db.Reservas.Add(reserva);
            _db.SaveChanges();
        }

        public List<Reserva> ObtenerPorEmailUsuario(string email)
        {
            return _db.Reservas
                .Include(r => r.Excursion)
                .Include(r => r.Usuario) 
                .Where(r => r.Usuario.Email == email)
                .OrderByDescending(r => r.FechaReserva)
                .ToList();
        }

        public List<Reserva> ObtenerPorExcursionId(int excursionId)
        {
            return _db.Reservas
                .Include(r => r.Usuario)
                .Where(r => r.IdExcursion == excursionId)
                .OrderByDescending(r => r.FechaReserva)
                .ToList();
        }

        public Reserva ObtenerPorId(int idReserva)
        {
            return _db.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Excursion)
                .FirstOrDefault(r => r.IdReserva == idReserva);
        }

        public void Actualizar(Reserva reserva)
        {
            _db.Reservas.Update(reserva);
            _db.SaveChanges();
        }

        public void Eliminar(int idReserva)
        {
            var reserva = _db.Reservas.Find(idReserva);
            if (reserva != null)
            {
                _db.Reservas.Remove(reserva);
                _db.SaveChanges();
            }
        }
    }
}
