using FitForge.Domain.Entities;

namespace FitForge.Application.Interfaces;

public interface IEjercicioRepository {
    Task<IEnumerable<EjercicioGlobal>> GetAllAsync();
    Task<EjercicioGlobal> AddAsync(EjercicioGlobal ejercicio); // Agregamos el contrato para el POST
}