using Excursiones.Data.Modelos;

namespace Excursiones.Data.Repositorios
{
    public interface IReservaRepository
    {
        void Insertar(Reserva reserva);
        List<Reserva> ObtenerPorEmailUsuario(string email);
        List<Reserva> ObtenerPorExcursionId(int excursionId);
        Reserva ObtenerPorId(int idReserva);
        void Eliminar(int idReserva);
        void Actualizar(Reserva reserva);
    }
}