using Msn.InteropDemo.Common.Utils.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace Msn.InteropDemo.ViewModel.Attributes.Validators
{
    public class FechaValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //Se retorna Success si es null porque probablemente podría ser opcional
            //Par que sea obligatororio la Property deberá ser convinada con [Required]
            if (value == null)
            {
                return ValidationResult.Success;
            }

            DateTime foo;
            if (DateTimeHelper.TryParseFromAR(value.ToString(), out foo) && foo > DateTime.Today.AddYears(-100))
            {
                return ValidationResult.Success;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(ErrorMessage))
                {
                    return new ValidationResult(ErrorMessage);
                }

                return new ValidationResult("La fecha ingresada es incorrecta.");
            }
        }
    }
}
