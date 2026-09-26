using Microsoft.EntityFrameworkCore;
using FitForge.Domain.Entities;

namespace FitForge.Infrastructure.Data;

public class FitForgeDbContext : DbContext {
    public FitForgeDbContext(DbContextOptions<FitForgeDbContext> options) : base(options) { }
    
    public DbSet<EjercicioGlobal> Ejercicios { get; set; }
}