using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PersonApi.utils
{
    public class Fix4960ActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var objectResult = context.Result as ObjectResult;
            if (objectResult?.Value is IActionResult) context.Result = (IActionResult) objectResult.Value;
        }
    }
}