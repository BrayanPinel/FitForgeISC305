using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FitForge.Application.Interfaces;
using FitForge.Infrastructure.Data;
using FitForge.Infrastructure.Repositories;

namespace FitForge.Infrastructure;

public static class DependencyInjection {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString) {
        
        // 1. Configuramos el acceso a PostgreSQL
        services.AddDbContext<FitForgeDbContext>(options =>
            options.UseNpgsql(connectionString));

        // 2. LA INYECCIÓN CLAVE (El contrato firmado por el Gerente):
        // "Cada vez que un Entrenador (Controlador) pida el Catálogo de Ejercicios (IEjercicioRepository),
        // entrégale los datos del Gimnasio PostgreSQL (EjercicioRepository)"
        services.AddScoped<IEjercicioRepository, EjercicioRepository>();

        return services;
    }
}