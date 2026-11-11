using Excursiones.Data.Modelos;
using Excursiones.Data.Repositorios;
using Excursiones.Web.Seguridad;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Excursiones.Web.Controllers
{
    // Esta etiqueta bloquea el acceso a toda persona que no haya iniciado sesión
    [Authorize]
    public class ReservasController : Controller
    {
        private readonly IExcursionRepository _excursionRepository;
        private readonly IReservaRepository _reservaRepository;
        private readonly ICalificacionRepository _calificacionRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public ReservasController(IExcursionRepository excursionRepository, IReservaRepository reservaRepository, ICalificacionRepository calificacionRepository, IUsuarioRepository usuarioRepository)
        {
            _excursionRepository = excursionRepository;
            _reservaRepository = reservaRepository;
            _calificacionRepository = calificacionRepository;
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet]
        [Authorize]
        [TieneAccion("CrearReservaPropia")]
        public IActionResult Crear(int id)
        {
            if (!User.HasClaim("Accion", "CrearReservaPropia"))
            {
                TempData["Info"] = "Estás navegando en modo Administrador. El flujo de reservas está habilitado únicamente para cuentas de clientes.";
                return RedirectToAction("Index", "Home");
            }

            var emailUsuario = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

            // Buscamos todas las reservas de este usuario
            var reservasDelUsuario = _reservaRepository.ObtenerPorEmailUsuario(emailUsuario);

            // Verificamos si alguna de esas reservas pertenece a la excursión que está intentando abrir
            var reservaExistente = reservasDelUsuario.FirstOrDefault(r => r.IdExcursion == id);

            if (reservaExistente != null)
            {
                // Si ya tiene una reserva, le avisamos y lo enviamos a la pantalla de edición
                TempData["Info"] = "Ya tienes una reserva activa para este viaje. Puedes modificar tus lugares aquí.";
                return RedirectToAction("Editar", new { idReserva = reservaExistente.IdReserva });
            }

            var excursion = _excursionRepository.ObtenerExcursionPorId(id);
            
            if (excursion == null || excursion.IdEstado != 1)
            {
                TempData["Error"] = "La excursión no está disponible.";
                return RedirectToAction("Index", "Home");
            }

            return View(excursion);
        }
    

        [HttpPost]
        [Authorize]
        public IActionResult GuardarReserva(int IdExcursion, int CantidadLugares)
        {
            //  Evita que el Admin guarde reservas
            if (!User.HasClaim("Accion", "CrearReservaPropia"))
            {
                TempData["Error"] = "Modo Administrador: No puedes ocupar lugares de excursiones reales.";
                return RedirectToAction("Index", "Home");
            }

            var emailUsuario = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var nombreUsuario = User.Identity.Name ?? "Sin Nombre";

            var idUsuarioClaim = User.FindFirst("IdUsuario")?.Value;

            if (string.IsNullOrEmpty(emailUsuario))
            {
                TempData["Error"] = "No se pudo identificar tu sesión de usuario. Por favor, vuelve a iniciar sesión.";
                return RedirectToAction("Index", "Login");
            }

            var usuarioDb = _usuarioRepository.ObtenerOCrearUsuario(emailUsuario, nombreUsuario);
            int idUsuarioLogueado = usuarioDb.IdPersona;

            // Buscar la excursión para verificar el cupo actual
            var excursion = _excursionRepository.ObtenerExcursionPorId(IdExcursion);

            if (excursion == null)
            {
                return NotFound();
            }

            // Validar que nadie más haya ocupado los lugares mientras el usuario completaba el formulario
            if (excursion.Cupo < CantidadLugares)
            {
                TempData["Error"] = "Lo sentimos, no hay suficientes lugares disponibles.";
                return RedirectToAction("Crear", new { id = IdExcursion });
            }

            var nuevaReserva = new Excursiones.Data.Modelos.Reserva
            {
                IdExcursion = IdExcursion,
                IdUsuario = idUsuarioLogueado,
                CantidadLugares = CantidadLugares,
                FechaReserva = System.DateTime.Now,
                TotalAbonado = excursion.Precio * CantidadLugares
                
            };
            
            excursion.Cupo -= CantidadLugares;

            _reservaRepository.Insertar(nuevaReserva); 
            _excursionRepository.ActualizarExcursion(excursion);
           

            TempData["Exito"] = "¡Reserva confirmada con éxito!";
            return RedirectToAction("MisReservas");
        }

        [HttpGet]
        [Authorize]
        [TieneAccion("CrearReservaPropia")]
        public IActionResult MisReservas()
        {
            var emailUsuario = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(emailUsuario))
            {
                return RedirectToAction("Index", "Home");
            }

            var misReservas = _reservaRepository.ObtenerPorEmailUsuario(emailUsuario);
            return View(misReservas);
        }


        //GET: Solo busca la reserva y muestra la página web visual
        [HttpGet]
        [Authorize]
        [TieneAccion("EditarReservaPropia")]
        public IActionResult Editar(int idReserva)
        {
            if (!User.HasClaim("Accion", "CrearReservaPropia"))
            {
                TempData["Info"] = "Estás navegando en modo Administrador. El flujo de reservas está habilitado únicamente para cuentas de clientes.";
                return RedirectToAction("Index", "Home");
            }

            var emailUsuario = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var reserva = _reservaRepository.ObtenerPorId(idReserva);

            if (reserva == null || reserva.Usuario.Email != emailUsuario)
            {
                TempData["Error"] = "No tienes permisos para editar esta reserva.";
                return RedirectToAction("MisReservas");
            }

            if (reserva.Excursion == null)
            {
                reserva.Excursion = _excursionRepository.ObtenerExcursionPorId(reserva.IdExcursion);
            }

            return View(reserva);
        }

        // POST: Recibe los cambios y hace los cálculos
        [HttpPost]
        [Authorize]
        public IActionResult Editar(int idReserva, int nuevaCantidadLugares)
        {
            
            if (!User.HasClaim("Accion", "CrearReservaPropia"))
            {
                TempData["Error"] = "Modo Administrador: No puedes modificar reservas de clientes.";
                return RedirectToAction("Index", "Home");
            }

            var emailUsuario = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var reserva = _reservaRepository.ObtenerPorId(idReserva);

            if (reserva == null || reserva.Usuario.Email != emailUsuario)
            {
                TempData["Error"] = "Reserva inválida o sin permisos.";
                return RedirectToAction("MisReservas");
            }

            var excursion = _excursionRepository.ObtenerExcursionPorId(reserva.IdExcursion);

            if (excursion == null || excursion.IdEstado != 1)
            {
                TempData["Error"] = "No puedes modificar una excursión que ya no está programada.";
                return RedirectToAction("MisReservas");
            }

            // Cálculo de diferencia 
            int diferencia = nuevaCantidadLugares - reserva.CantidadLugares;

            if (diferencia == 0)
            {
                TempData["Exito"] = "No se realizaron cambios en tu reserva.";
                return RedirectToAction("MisReservas");
            }

            // Evaluar si suma o resta lugares
            if (diferencia > 0) 
            {
                if (excursion.Cupo < diferencia)
                {
                    TempData["Error"] = $"No hay suficientes lugares. Solo quedan {excursion.Cupo} disponibles.";
                    return RedirectToAction("Editar", new { idReserva = idReserva });
                }
                excursion.Cupo -= diferencia;
            }
            else 
            {
                if (nuevaCantidadLugares <= 0)
                {
                    TempData["Error"] = "La reserva debe tener al menos 1 lugar. Si deseas cancelar todo, usa el botón de Cancelar.";
                    return RedirectToAction("Editar", new { idReserva = idReserva });
                }
                excursion.Cupo += Math.Abs(diferencia);
            }

            // Actualizar los datos de la reserva 
            reserva.CantidadLugares = nuevaCantidadLugares;
            reserva.TotalAbonado = nuevaCantidadLugares * excursion.Precio;

            _excursionRepository.ActualizarExcursion(excursion);
            _reservaRepository.Actualizar(reserva);

            TempData["Exito"] = "Tu reserva se ha actualizado correctamente.";
            return RedirectToAction("MisReservas");
        }


        [HttpPost]
        [Authorize]
        public IActionResult Cancelar(int idReserva)
        {
          
            if (!User.HasClaim("Accion", "CrearReservaPropia"))
            {
                TempData["Error"] = "Modo Administrador: No puedes cancelar reservas.";
                return RedirectToAction("Index", "Home");
            }

            var emailUsuario = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var reserva = _reservaRepository.ObtenerPorId(idReserva);

   
            if (reserva == null || reserva.Usuario.Email != emailUsuario)
            {
                TempData["Error"] = "No se encontró la reserva o no tienes permisos.";
                return RedirectToAction("MisReservas");
            }

            if (reserva.Excursion.Borrado || reserva.Excursion.IdEstado == 3)
            {
                _reservaRepository.Eliminar(idReserva);
                TempData["Exito"] = "La reserva ha sido quitada de tu historial.";
                return RedirectToAction("MisReservas");
            }


            if (reserva.Excursion.IdEstado == 2)
            {
                TempData["Error"] = "No puedes cancelar una excursión que ya fue completada.";
                return RedirectToAction("MisReservas");
            }
            
            if (reserva.Excursion.IdEstado != 1)
            {
                TempData["Error"] = "No puedes cancelar una excursión que ya fue completada o suspendida.";
                return RedirectToAction("MisReservas");
            }

            var horasRestantes = (reserva.Excursion.FechaExcursion - DateTime.Now).TotalHours;
            if (horasRestantes < 48)
            {
                TempData["Error"] = "Solo puedes cancelar con al menos 48 horas de anticipación a la fecha de salida.";
                return RedirectToAction("MisReservas");
            }

       
            var excursion = _excursionRepository.ObtenerExcursionPorId(reserva.IdExcursion);
            excursion.Cupo += reserva.CantidadLugares;

            _excursionRepository.ActualizarExcursion(excursion);
            _reservaRepository.Eliminar(idReserva);

            TempData["Exito"] = "Reserva cancelada correctamente. Se ha liberado tu cupo.";
            return RedirectToAction("MisReservas");
        }


        [HttpGet]
        [Authorize]
        public IActionResult Calificar(int idReserva)
        {
            var emailUsuario = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var reserva = _reservaRepository.ObtenerPorId(idReserva);

            // Validar que la reserva sea del usuario y la excursión esté completada
            if (reserva == null || reserva.Usuario.Email != emailUsuario || reserva.Excursion.IdEstado != 2)
            {
                TempData["Error"] = "No tienes permisos para calificar este viaje.";
                return RedirectToAction("MisReservas");
            }

            //  Evitar que califiquen dos veces la misma reserva
            if (_calificacionRepository.ExisteCalificacion(idReserva))
            {
                TempData["Error"] = "Ya has calificado esta excursión anteriormente. ¡Gracias por tu opinión!";
                return RedirectToAction("MisReservas");
            }

            return View(reserva);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Calificar(int idReserva, int puntuacion, string comentario)
        {
         
            if (!User.HasClaim("Accion", "CrearReservaPropia"))
            {
                TempData["Error"] = "Modo Administrador: No puedes emitir calificaciones.";
                return RedirectToAction("Index", "Home");
            }

            if (puntuacion < 1 || puntuacion > 5)
            {
                TempData["Error"] = "Por favor, selecciona una puntuación válida de 1 a 5 estrellas.";
                return RedirectToAction("Calificar", new { idReserva = idReserva });
            }

            var calificacion = new Excursiones.Data.Modelos.Calificacion
            {
                IdReserva = idReserva,
                Puntuacion = puntuacion,
                Comentario = comentario,
                Fecha = System.DateTime.Now
            };

            _calificacionRepository.Insertar(calificacion);

            TempData["Exito"] = "¡Tu calificación ha sido guardada! Gracias por compartir tu experiencia.";
            return RedirectToAction("MisReservas");
        }
    }
}