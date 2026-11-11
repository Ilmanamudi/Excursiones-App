using Excursiones.Data.Db;
using Excursiones.Data.Modelos;

namespace Excursiones.Data.Db
{
    public static class DbInitializer
    {
        public static void Initialize(ExcursionesContext context)
        {
            
            context.Database.EnsureCreated();
           
            if (!context.Roles.Any())
            {
              
                var rolAdmin = new Rol { Descripcion = "Administrador" };
                var rolCliente = new Rol { Descripcion = "Cliente" };

                context.Roles.AddRange(rolAdmin, rolCliente);
                context.SaveChanges(); 

                // Admin 
                var accionAccederPanelAdmin = new Accion { Descripcion = "AccederPanelAdmin" };
                var accionCrearExcursion = new Accion { Descripcion = "CrearExcursion" };
                var accionEditarExcursion = new Accion { Descripcion = "EditarExcursion" };
                var accionCambiarEstadoExcursion = new Accion { Descripcion = "CambiarEstadoExcursion" };
                var accionVerReservasTotales = new Accion { Descripcion = "VerReservasTotales" };

                //Cliente
                var accionReservar = new Accion { Descripcion = "CrearReservaPropia" };
                var accionEditarReserva = new Accion { Descripcion = "EditarReservaPropia" };
                var accionCancelarReserva = new Accion { Descripcion = "CancelarReservaPropia" };
                var accionCalificarExcursion = new Accion { Descripcion = "CalificarExcursion" };

                context.Acciones.AddRange(
                    accionAccederPanelAdmin,
                    accionCrearExcursion,
                    accionEditarExcursion,
                    accionCambiarEstadoExcursion,
                    accionVerReservasTotales,
                    accionReservar,
                    accionEditarReserva,
                    accionCancelarReserva,
                    accionCalificarExcursion
                );
                context.SaveChanges(); 

                // Vincular Roles con Acciones 
                // El Administrador
                context.RolesAcciones.Add(new RolAccion { IdRol = rolAdmin.IdRol, IdAccion = accionAccederPanelAdmin.IdAccion });
                context.RolesAcciones.Add(new RolAccion { IdRol = rolAdmin.IdRol, IdAccion = accionCrearExcursion.IdAccion });
                context.RolesAcciones.Add(new RolAccion { IdRol = rolAdmin.IdRol, IdAccion = accionEditarExcursion.IdAccion });
                context.RolesAcciones.Add(new RolAccion { IdRol = rolAdmin.IdRol, IdAccion = accionCambiarEstadoExcursion.IdAccion });
                context.RolesAcciones.Add(new RolAccion { IdRol = rolAdmin.IdRol, IdAccion = accionVerReservasTotales.IdAccion });

                // El Cliente 
                context.RolesAcciones.Add(new RolAccion { IdRol = rolCliente.IdRol, IdAccion = accionReservar.IdAccion });
                context.RolesAcciones.Add(new RolAccion { IdRol = rolCliente.IdRol, IdAccion = accionEditarReserva.IdAccion });
                context.RolesAcciones.Add(new RolAccion { IdRol = rolCliente.IdRol, IdAccion = accionCancelarReserva.IdAccion });
                context.RolesAcciones.Add(new RolAccion { IdRol = rolCliente.IdRol, IdAccion = accionCalificarExcursion.IdAccion });
                

                context.SaveChanges();
            }
            // Estados de Excursión
            if (!context.EstadosExcursion.Any())
            {
                context.EstadosExcursion.AddRange(
                   new EstadoExcursion { NombreEstado = "Programada" },
                   new EstadoExcursion { NombreEstado = "Completada" },
                   new EstadoExcursion { NombreEstado = "Cancelada" }
                );
                context.SaveChanges();
            }
        }
    }
}