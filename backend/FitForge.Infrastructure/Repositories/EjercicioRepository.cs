using Microsoft.EntityFrameworkCore;
using FitForge.Application.Interfaces;
using FitForge.Domain.Entities;
using FitForge.Infrastructure.Data;

namespace FitForge.Infrastructure.Repositories;

public class EjercicioRepository : IEjercicioRepository {
    private readonly FitForgeDbContext _context;
    
    public EjercicioRepository(FitForgeDbContext context) {
        _context = context;
    }

    public async Task<IEnumerable<EjercicioGlobal>> GetAllAsync() {
        return await _context.Ejercicios.ToListAsync();
    }
}