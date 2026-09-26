using FitForge.Domain.Entities;

namespace FitForge.Application.Interfaces;

public interface IEjercicioRepository {
    Task<IEnumerable<EjercicioGlobal>> GetAllAsync();
}