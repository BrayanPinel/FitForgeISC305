namespace FitForge.Application.Features.Ejercicios.DTOs;

public class CreateEjercicioRequestDto {
    public string Nombre { get; set; } = string.Empty;
    public string GrupoMuscular { get; set; } = string.Empty;
}