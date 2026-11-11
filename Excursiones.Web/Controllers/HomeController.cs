using Excursiones.Data.Repositorios;
using Excursiones.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Excursiones.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly IExcursionRepository _excursionRepository;
        public HomeController(IExcursionRepository excursionRepository)
        {
            _excursionRepository = excursionRepository;
        }
        public IActionResult Index()
        {
            // Actualizamos los estados caducados automáticamente
            _excursionRepository.ActualizarEstadosAutomaticamente();
            var proximasExcursiones = _excursionRepository.ObtenerExcursiones()
                    .Where(e => e.Borrado == false && e.IdEstado == 1)
                    .OrderBy(e => e.FechaExcursion)
                    .Select(e => new Excursiones.Web.Models.ExcursionVM
                    {
                        IdExcursion = e.IdExcursion,
                        NombreExcursion = e.NombreExcursion,
                        FechaExcursion = e.FechaExcursion,
                        Precio = e.Precio,
                        Cupo = e.Cupo,
                        DuracionDias = e.DuracionDias,
                        Descripcion = e.Descripcion,
                        IdEstado = e.IdEstado,
                        ImagenUrl = string.IsNullOrEmpty(e.ImagenUrl) ? "/images/default-excursion.png" : e.ImagenUrl
                    })
                    .ToList();

            // Enviamos la lista a la vista
            return View(proximasExcursiones);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult AccesoDenegado()
        {
            ViewBag.Mensaje = "Operación realizada con éxito";
            return View();
        }
    }
}
