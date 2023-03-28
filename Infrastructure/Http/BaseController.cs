using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PhotoPicker.Infrastructure.Http
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!ModelState.IsValid)
                context.Result = new BadRequestObjectResult(ModelState);
            base.OnActionExecuting(context);
        }

        protected string GetClaim(string claimType) => HttpContext.User.Claims.Where(x => x.Type == claimType).First().Value;

        protected ServerErrorResult InternalServerError() => new ServerErrorResult();
        protected ServerErrorObjectResult InternalServerError(object? value) => new ServerErrorObjectResult(value);
    }
}
