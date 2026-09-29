using System.ComponentModel.DataAnnotations;
using todolistnew.Api.Models.Request;

namespace todolistnew.Api.Validation;

public class DateTimeAtrebyte : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is CreateToDoModel request)
            if (request.DateStart > request.DateEnd)
                return new ValidationResult("Дата начала не может быть позже даты окончания");

        return ValidationResult.Success;
    }
}