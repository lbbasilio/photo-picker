using Microsoft.AspNetCore.Mvc;

namespace PhotoPicker.Infrastructure.Http
{
    public class ServerErrorResult : StatusCodeResult
    {
        public ServerErrorResult() : base(500) { }
    }

    public class ServerErrorObjectResult : ObjectResult
    {
        public ServerErrorObjectResult(object? value) : base(value) => StatusCode = 500;
    }
}