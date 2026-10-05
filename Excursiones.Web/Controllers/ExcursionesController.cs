using Excursiones.Data.Modelos;
using Excursiones.Data.Repositorios;
using Excursiones.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Excursiones.Web.Controllers
{
    public class ExcursionesController : Controller
    {
        private readonly IExcursionRepository _excursionRepository;

        public ExcursionesController(IExcursionRepository excursionRepository)
        {
            _excursionRepository = excursionRepository;
        }

        // GET: Excursiones
        public ActionResult Index()
        {
            var excursionesddbb = _excursionRepository.ObtenerExcursiones();
           
            var excursiones = excursionesddbb
                .Where(x => x.Borrado == false)
                .Select(x => new ExcursionVM
                {
                    IdExcursion = x.IdExcursion,
                    NombreExcursion = x.NombreExcursion,
                    FechaExcursion = x.FechaExcursion,
                    Precio = x.Precio,
                    Cupo = x.Cupo,
                    IdEstado = x.IdEstado,
                    Descripcion = x.Descripcion,
                    DuracionDias = x.DuracionDias


                }).ToList();

            foreach (var vm in excursiones)
            {
                vm.NombreEstado = vm.IdEstado == 1 ? "Programada" : (vm.IdEstado == 2 ? "Completada" : "Cancelada");
                vm.ColorEstado = vm.IdEstado == 1 ? "bg-primary" : (vm.IdEstado == 2 ? "bg-success" : "bg-danger");
            }

            var estados = _excursionRepository.ObtenerEstadosExcursion();
            ViewBag.Estados = new SelectList(estados, "IdEstado", "NombreEstado");

            return View(excursiones);
        }

        // POST: Excursiones/Create
        [HttpPost]
        public IActionResult Create(ExcursionVM excursion)
        {
            if (excursion.FechaExcursion.Date < DateTime.Now.Date)
            {
                ModelState.AddModelError("FechaExcursion", "La fecha de salida no puede ser en el pasado.");
            }


            if (!ModelState.IsValid)
            {
                var primerError = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
                TempData["Mensaje"] = $"Error: {primerError}";
                return RedirectToAction(nameof(Index));
            }


            var nuevaExcursion = new Excursion
            {
                NombreExcursion = excursion.NombreExcursion,
                FechaExcursion = excursion.FechaExcursion,
                Precio = excursion.Precio,
                Cupo = excursion.Cupo,
                IdEstado = excursion.IdEstado,
                Descripcion = excursion.Descripcion,
                DuracionDias = excursion.DuracionDias,
                Borrado = false
            };

            bool guardadoExitoso = _excursionRepository.AgregarExcursion(nuevaExcursion);

            if (guardadoExitoso)
            {
                TempData["Exito"] = "La excursión se guardó correctamente.";
            }
            else
            {
                TempData["Error"] = "Hubo un problema al procesar la solicitud.";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Excursiones/Edit
        [HttpPost]
        public ActionResult Edit(ExcursionVM excursion)
        {
            if (excursion.FechaExcursion.Date < DateTime.Now.Date)
            {
                ModelState.AddModelError("FechaExcursion", "No puedes reprogramar una excursión al pasado.");
            }

            if (!ModelState.IsValid)
            {
                var primerError = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
                TempData["Error"] = $"Error al editar: {primerError}";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var excursionAModificar = _excursionRepository.ObtenerExcursionPorId(excursion.IdExcursion);

                if (excursionAModificar != null)
                {
                    
                    excursionAModificar.NombreExcursion = excursion.NombreExcursion;
                    excursionAModificar.FechaExcursion = excursion.FechaExcursion;
                    excursionAModificar.Precio = excursion.Precio;
                    excursionAModificar.Cupo = excursion.Cupo;
                    excursionAModificar.IdEstado = excursion.IdEstado;
                    excursionAModificar.Descripcion = excursion.Descripcion;
                    excursionAModificar.DuracionDias = excursion.DuracionDias;

                    _excursionRepository.ActualizarExcursion(excursionAModificar);
                    TempData["Mensaje"] = "Excursión actualizada correctamente.";
                }
                else
                {
                    TempData["Mensaje"] = "Error: No se encontró la excursión seleccionada.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Mensaje"] = "Ocurrió un error al intentar actualizar.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Excursiones/Delete/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id) 
        {
            try
            {
                var excursion = _excursionRepository.ObtenerExcursionPorId(id);

                if (excursion != null)
                {
                    excursion.Borrado = true;
                    _excursionRepository.ActualizarExcursion(excursion);

                    TempData["Exito"] = "Excursión eliminada correctamente.";
                }
                else
                {
                  
                    TempData["Error"] = "No se encontró la excursión que intentas eliminar.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Ocurrió un error al intentar eliminar.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult ObtenerModalEditar(int id)
        {
         
            var excursion = _excursionRepository.ObtenerExcursionPorId(id);

           
            if (excursion == null)
            {
                return NotFound();
            }
         
            var vm = new ExcursionVM
            {
                IdExcursion = excursion.IdExcursion,
                NombreExcursion = excursion.NombreExcursion,
                FechaExcursion = excursion.FechaExcursion,
                Precio = excursion.Precio,
                Cupo = excursion.Cupo,
                DuracionDias = excursion.DuracionDias,
                IdEstado = excursion.IdEstado,
                Descripcion = excursion.Descripcion
            };

         
            var listaEstados = new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "Programada" },
                    new SelectListItem { Value = "2", Text = "Completada" },
                    new SelectListItem { Value = "3", Text = "Cancelada" }
                };
            ViewBag.Estados = new SelectList(listaEstados, "Value", "Text");

            return PartialView("_ModalEdit", vm);
        }
    }
}