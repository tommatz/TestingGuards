using FluentValidation;

namespace TestingFluentValidation;

public class ValidationWrapper
{
    public static void Validate<T>(T obj, AbstractValidator<T> validator, string? ruleContext = null)
    {
        var result = ruleContext != null
            ? validator.Validate(obj, options => options.IncludeRuleSets(ruleContext))
            : validator.Validate(obj);

        if (!result.IsValid)
        {
            var errorMessages = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));

            throw new ValidationException($"Validation failed: {errorMessages}");
        }
    }
}