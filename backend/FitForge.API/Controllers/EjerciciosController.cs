using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using FitForge.Application.Interfaces;
using FitForge.Domain.Entities;
using FitForge.Application.Features.Ejercicios.DTOs;

namespace FitForge.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EjerciciosController : ControllerBase {
    private readonly IEjercicioRepository _repository;
    private readonly IValidator<CreateEjercicioRequestDto> _validator;

    public EjerciciosController(IEjercicioRepository repository, IValidator<CreateEjercicioRequestDto> validator) {
        _repository = repository;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetEjercicios() {
        var ejercicios = await _repository.GetAllAsync();
        
        // Mapeo Manual: Entidad -> DTO
        var response = ejercicios.Select(e => new EjercicioResponseDto {
            Id = e.Id,
            Nombre = e.Nombre,
            GrupoMuscular = e.GrupoMuscular
        });

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CrearEjercicio([FromBody] CreateEjercicioRequestDto request) {
        // 1. Ejecutar FluentValidation
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid) {
            return BadRequest(validationResult.Errors); // HTTP 400 automático con detalles
        }

        // 2. Mapeo Manual: DTO -> Entidad (Solo transferimos lo permitido)
        var nuevoEjercicio = new EjercicioGlobal {
            Nombre = request.Nombre,
            GrupoMuscular = request.GrupoMuscular
        };

        // 3. Persistencia
        var ejercicioCreado = await _repository.AddAsync(nuevoEjercicio);
        
        // 4. Mapeo de Retorno
        var response = new EjercicioResponseDto {
            Id = ejercicioCreado.Id,
            Nombre = ejercicioCreado.Nombre,
            GrupoMuscular = ejercicioCreado.GrupoMuscular
        };

        return CreatedAtAction(nameof(GetEjercicios), new { id = response.Id }, response);
    }
}