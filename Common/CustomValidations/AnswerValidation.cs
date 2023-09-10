using System.ComponentModel.DataAnnotations;

namespace Common.CustomValidation;

public class ValidAnswer : ValidationAttribute
{
  protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
  {
    return (string)value != "A" ? new ValidationResult("Answer accepts only A, B, C, D, E, V, or F") :
    ValidationResult.Success;
  }
}