using Excursiones.Data.Modelos;
using Excursiones.Data.Repositorios;
using Excursiones.Web.Models;
using Excursiones.Web.Seguridad;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Excursiones.Web.Controllers
{
    [Authorize]
    public class ExcursionesController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IExcursionRepository _excursionRepository;
        private readonly IReservaRepository _reservaRepository;

        public ExcursionesController(IExcursionRepository excursionRepository, IReservaRepository reservaRepository, IWebHostEnvironment webHostEnvironment)
        {
            _excursionRepository = excursionRepository;
            _reservaRepository = reservaRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Excursiones
        [HttpGet]
        [Authorize]
        [TieneAccion("AccederPanelAdmin")]
        public ActionResult Index()
        {
            // Actualizamos los estados automáticamente
            _excursionRepository.ActualizarEstadosAutomaticamente();
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

        [HttpGet]
        [TieneAccion("VerReservasTotales")]
        public IActionResult ObtenerPasajeros(int id)
        {
            var reservas = _reservaRepository.ObtenerPorExcursionId(id);

          
            return PartialView("_TablaPasajeros", reservas);
        }

     
        
        [HttpPost]
        [TieneAccion("CrearExcursion")]
        public IActionResult Create(ExcursionVM excursion)
        {
            if (excursion.FechaExcursion.Date < DateTime.Now.Date)
            {
                ModelState.AddModelError("FechaExcursion", "La fecha de salida no puede ser en el pasado.");
            }

            if (!ModelState.IsValid)
            {
                var primerError = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
                TempData["Error"] = $"Error: {primerError}";
                return RedirectToAction(nameof(Index));
            }


           
                string imagenPath = "/images/default-excursion.png"; // Imagen por defecto por si no sube nada

                // Verificamos si el usuario seleccionó un archivo
                if (excursion.ImagenFile != null && excursion.ImagenFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "excursiones");

                    // Si la carpeta no existe, la creamos
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Creamos un nombre único para evitar que dos fotos se llamen igual y se sobrescriban
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(excursion.ImagenFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        excursion.ImagenFile.CopyTo(fileStream);
                    }

                    imagenPath = "/images/excursiones/" + uniqueFileName;
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
                    ImagenUrl = imagenPath, 
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
        
        
        [HttpPost]
        [TieneAccion("EditarExcursion")]
        public ActionResult Edit(ExcursionVM excursion)
        {
            
            var excursionAModificar = _excursionRepository.ObtenerExcursionPorId(excursion.IdExcursion);

            if (excursionAModificar == null)
            {
                TempData["Error"] = "Error: No se encontró la excursión seleccionada.";
                return RedirectToAction(nameof(Index));
            }

            bool esPasadaYHhistorica = excursionAModificar.IdEstado == 2 && excursionAModificar.FechaExcursion.Date < DateTime.Now.Date;
            //  Si ya estaba completada (IdEstado == 2) o su fecha ya pasó, se prohíbe editar
            if (esPasadaYHhistorica)
            {
                TempData["Error"] = "No se puede editar una excursión que ya ha sido completada o cuya fecha de salida ya ocurrió.";
                return RedirectToAction(nameof(Index));
            }

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
                    TempData["Exito"] = "Excursión actualizada correctamente.";
                }
                else
                {
                    TempData["Error"] = "Error: No se encontró la excursión seleccionada.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Ocurrió un error al intentar actualizar.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Excursiones/Delete/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        [TieneAccion("EditarExcursion")]
        public ActionResult Delete(int id) 
        {
            try
            {
                var excursion = _excursionRepository.ObtenerExcursionPorId(id);

                if (excursion != null)
                {
                    excursion.Borrado = true;
                    excursion.IdEstado = 3;
                    _excursionRepository.ActualizarExcursion(excursion);

                    TempData["Exito"] = "Excursión eliminada y suspendida correctamente.";
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
        [TieneAccion("EditarExcursion")]
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


            var estadosDb = _excursionRepository.ObtenerEstadosExcursion().ToList();
            
            ViewBag.Estados = new SelectList(estadosDb, "IdEstado", "NombreEstado");

            return PartialView("_ModalEdit", vm);
        }
    }
}