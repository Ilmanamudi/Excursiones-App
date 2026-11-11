using Excursiones.Data.Db;
using Excursiones.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Excursiones.Data.Repositorios
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ExcursionesContext _db;

        public UsuarioRepository(ExcursionesContext db)
        {
            _db = db;
        }
        public List<String> ObtenerAcciones(string Email)
        {
            var usuario = _db.Usuarios.Include(u => u.UsuarioRoles)
                                .ThenInclude(ur => ur.Rol)
                                .ThenInclude(r => r.RolesAcciones)
                                .ThenInclude(ra => ra.Accion)
                                .FirstOrDefault(u => u.Email == Email);

            if (usuario == null || usuario.UsuarioRoles == null)
            {
                return new List<string>();
            }

            var acciones = usuario.UsuarioRoles
                                  .Where(ur => ur.Rol != null && ur.Rol.RolesAcciones != null)
                                  .SelectMany(ur => ur.Rol.RolesAcciones)
                                  .Where(ra => ra.Accion != null)
                                  .Select(ra => ra.Accion.Descripcion)
                                  .Distinct()
                                  .ToList();

            return acciones;
        }
        public Usuario ObtenerOCrearUsuario(string email, string nombre)
        {
            var usuario = _db.Usuarios.Include(u => u.UsuarioRoles)
                              .FirstOrDefault(u => u.Email == email);

            if (usuario == null)
            {
                
                bool esElPrimerUsuario = !_db.Usuarios.Any();

                usuario = new Usuario
                {
                    Email = email,
                    Nombre = nombre,
                    FechaAlta = DateTime.Now
                };

                _db.Usuarios.Add(usuario);

                
                string descripcionRolDeseada = esElPrimerUsuario ? "Administrador" : "Cliente";
                var rol = _db.Roles.FirstOrDefault(r => r.Descripcion == descripcionRolDeseada);

                
                if (rol == null)
                {
                    rol = new Rol { Descripcion = descripcionRolDeseada };
                    _db.Roles.Add(rol);
                    _db.SaveChanges(); 
                }

               
                var usuarioRol = new UsuarioRol
                {
                    Usuario = usuario,
                    Rol = rol
                };

                _db.UsuarioRoles.Add(usuarioRol);
                _db.SaveChanges(); 
            }


            return usuario;
        }
           
    }
}
