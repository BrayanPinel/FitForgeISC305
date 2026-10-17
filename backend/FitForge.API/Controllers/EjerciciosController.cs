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
    
    private readonly IValidator<UpdateEjercicioRequestDto> _updateValidator;

    public EjerciciosController(IEjercicioRepository repository, IValidator<CreateEjercicioRequestDto> validator, IValidator<UpdateEjercicioRequestDto> updateValidator) {
    _repository = repository;
    _validator = validator;
    _updateValidator = updateValidator;
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

    // GET - Obtener un ejercicio por su ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEjercicioPorId(int id) {
        var ejercicio = await _repository.GetByIdAsync(id);
        if (ejercicio is null) {
            return NotFound(); // HTTP 404 Not Found
        }

        var response = new EjercicioResponseDto {
            Id = ejercicio.Id,
            Nombre = ejercicio.Nombre,
            GrupoMuscular = ejercicio.GrupoMuscular
        };

        return Ok(response); // HTTP 200 OK
    }

    // PUT - Actualizar un ejercicio existente
    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarEjercicio(int id, [FromBody] UpdateEjercicioRequestDto request) {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid) {
            return BadRequest(validationResult.Errors); // HTTP 400 Bad Request
        }

        var ejercicioExistente = await _repository.GetByIdAsync(id);
        if (ejercicioExistente is null) {
            return NotFound(); // HTTP 404 Not Found
        }

        ejercicioExistente.Nombre = request.Nombre;
        ejercicioExistente.GrupoMuscular = request.GrupoMuscular;

        await _repository.UpdateAsync(ejercicioExistente);

        return NoContent(); // HTTP 204 No Content
    }

    // DELETE - Eliminar un ejercicio existente
    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarEjercicio(int id) {
        var ejercicio = await _repository.GetByIdAsync(id);
        if (ejercicio is null) {
            return NotFound(); // HTTP 404 Not Found
        }

        await _repository.DeleteAsync(ejercicio);

        return NoContent(); // HTTP 204 No Content
    }
}