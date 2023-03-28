using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PhotoPicker.Controllers
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
    }
}
