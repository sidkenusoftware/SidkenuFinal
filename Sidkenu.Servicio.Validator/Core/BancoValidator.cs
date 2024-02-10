﻿using FluentValidation;
using Sidkenu.Servicio.DTOs.Core.Banco;

namespace Sidkenu.Servicio.Validator.Core
{
    public class BancoValidator : AbstractValidator<BancoPersistenciaDTO>
    {
        public BancoValidator()
        {
            RuleFor(x => x.Codigo)
                .NotEmpty().WithMessage("La {PropertyName} no puede estar vacía")
                .MaximumLength(10).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.");

            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La {PropertyName} no puede estar vacía")
                .MaximumLength(250).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.");
        }
    }
}