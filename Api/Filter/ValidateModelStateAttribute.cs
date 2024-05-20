using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filter;

public class ValidateModelStateAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values
               .Where(v => v.Errors.Count > 0)
               .SelectMany(v => v.Errors)
               .Select(v => v.ErrorMessage)
               .ToList();

            var responseObj = new
            {
                Message = "Validation Failed",
                Errors = errors
            };

            context.Result = new JsonResult(responseObj)
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity
            };
        }
    }
}