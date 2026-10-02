using FluentValidation;
using FitForge.Application.Features.Ejercicios.DTOs;

namespace FitForge.Application.Features.Ejercicios.Validators;

public class CreateEjercicioValidator : AbstractValidator<CreateEjercicioRequestDto> {
    public CreateEjercicioValidator() {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del ejercicio no puede estar vacío.")
            .MinimumLength(3).WithMessage("El ejercicio debe tener al menos 3 caracteres.")
            .MaximumLength(100).WithMessage("El nombre del ejercicio es demasiado largo.");

        RuleFor(x => x.GrupoMuscular)
            .NotEmpty().WithMessage("El grupo muscular no puede estar vacío.")
            .MinimumLength(3).WithMessage("El grupo muscular debe tener al menos 3 caracteres.")
            .MaximumLength(800).WithMessage("El grupo muscular es demasiado largo.");
    }
}