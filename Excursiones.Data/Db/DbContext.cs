using Excursiones.Data.Modelos;
using Microsoft.EntityFrameworkCore;

namespace Excursiones.Data.Db
{
    public class ExcursionesContext : DbContext
    {
        public ExcursionesContext(DbContextOptions<ExcursionesContext> options) : base(options) { }

        public DbSet<Excursion> Excursiones => Set<Excursion>();
        public DbSet<EstadoExcursion> EstadosExcursion => Set<EstadoExcursion>();
    }
}

