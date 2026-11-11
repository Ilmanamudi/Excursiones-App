using Excursiones.Data.Modelos;
using Microsoft.EntityFrameworkCore;

namespace Excursiones.Data.Db
{
    public class ExcursionesContext : DbContext
    {
        public ExcursionesContext(DbContextOptions<ExcursionesContext> options) : base(options) { }

        public DbSet<Excursion> Excursiones => Set<Excursion>();
        public DbSet<EstadoExcursion> EstadosExcursion => Set<EstadoExcursion>();
        public DbSet<Reserva> Reservas { get; set; }

        public DbSet<Calificacion> Calificaciones { get; set; }

        public DbSet<Usuario> Usuarios => Set<Usuario>();

        public DbSet<UsuarioRol> UsuarioRoles => Set<UsuarioRol>();
        public DbSet<Rol> Roles => Set<Rol>();
        public DbSet<RolAccion> RolesAcciones => Set<RolAccion>();
        public DbSet<Accion> Acciones => Set<Accion>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}

