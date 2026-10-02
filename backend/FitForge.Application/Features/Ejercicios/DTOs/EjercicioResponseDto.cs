namespace FitForge.Application.Features.Ejercicios.DTOs;

public class EjercicioResponseDto {
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string GrupoMuscular { get; set; } = string.Empty;
}