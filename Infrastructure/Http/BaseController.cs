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
        protected ServerErrorResult ServerError() => new ServerErrorResult();
        protected ServerErrorObjectResult ServerError(object? value) => new ServerErrorObjectResult(value);
        protected ServerErrorObjectResult HandleException(Exception ex)
        {

            var message = $"{ex.Message} - {ex.StackTrace}";
            if (ex.InnerException is not null)
                message += $"\n#####\n{ex.InnerException.Message} - {ex.InnerException.StackTrace}";
            
            Console.WriteLine(message);
            return ServerError(message);
        }
    }
}
