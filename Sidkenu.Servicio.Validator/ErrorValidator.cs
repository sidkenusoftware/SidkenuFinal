using FluentValidation.Results;

namespace Sidkenu.Servicio.Validator
{
    public static class ErrorValidator
    {
        public static string ObtenerErrores(ValidationResult result)
        {
            var errores = "Ocurrió un error al guardar los datos" + Environment.NewLine + Environment.NewLine;

            foreach (var error in result.Errors)
            {
                errores += $"{error.ErrorMessage}" + Environment.NewLine;
            }

            return errores;
        }

        public static string ObtenerErrores(IEnumerable<ValidationFailure> validationFailures)
        {
            var errores = "Ocurrió un error al guardar los datos" + Environment.NewLine + Environment.NewLine;

            foreach (var error in validationFailures)
            {
                errores += $"{error.ErrorMessage}" + Environment.NewLine;
            }

            return errores;
        }
    }
}
