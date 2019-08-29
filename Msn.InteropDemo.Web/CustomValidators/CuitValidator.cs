using System.ComponentModel.DataAnnotations;

namespace Msn.InteropDemo.Web.CustomValidators
{
    public class CuitValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //Se retorna Success si es null porque probablemente podría ser opcional
            //Para que sea obligatororio la Property deberá ser combinada con [Required]
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (Common.Utils.Helpers.Cuit.Validate(value.ToString()))
            {
                return ValidationResult.Success;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(ErrorMessage))
                {
                    return new ValidationResult(ErrorMessage);
                }

                return new ValidationResult("El CUIT ingresado es incorrecto.");
            }
        }
    }
}
