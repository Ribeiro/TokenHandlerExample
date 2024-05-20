using Api;
using FluentValidation;

namespace Api.Validation;

public class WeatherForecastValidator : AbstractValidator<WeatherForecast>
{
    public WeatherForecastValidator()
    {
        RuleFor(p => p.Date)
           .Must(BeInTheFuture)
           .WithMessage("Date of birth must be in the future.");
        RuleFor(dto => dto.TemperatureC).NotEmpty().WithMessage("TemperatureC is required.");
        RuleFor(dto => dto.Summary).NotEmpty().WithMessage("Summary is required.");
    }

    private bool BeInTheFuture(DateTime? date)
    {
        return!date.HasValue || date.Value >= DateTime.Now;
    }
}