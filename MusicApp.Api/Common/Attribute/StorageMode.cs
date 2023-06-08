using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MusicApp.Api.Common.Attribute;

public class StorageMode : ActionFilterAttribute
{
    private readonly bool cloudMode;
    public StorageMode(IConfiguration configuration)
    {
        cloudMode = configuration.GetValue<bool>("Storage:CloudMode");
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    { 

        if (cloudMode)
        {
            context.Result = new NotFoundResult(); 
        }
        base.OnActionExecuting(context);
    }


}
