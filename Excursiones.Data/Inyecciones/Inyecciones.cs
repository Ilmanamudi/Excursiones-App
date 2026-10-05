using Excursiones.Data.Db;
using Excursiones.Data.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Excursiones.Data.Inyecciones
{
    public static class DataAccessExtensions
    {
        public static IServiceCollection AddDataAccess(
           this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ExcursionesContext>(o =>
                o.UseSqlServer(connectionString));
            services.AddScoped<IExcursionRepository, ExcursionRepository>();
            return services;
        }

    }
}
