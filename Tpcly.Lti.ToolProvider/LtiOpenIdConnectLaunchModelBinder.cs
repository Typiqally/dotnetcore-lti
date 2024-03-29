using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Tpcly.Lti.ToolProvider;

public class LtiOpenIdConnectLaunchModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var form = bindingContext.HttpContext.Request.Form;
        var launchRequest = new LtiOpenIdConnectInitiation(
            form["iss"],
            form["login_hint"],
            form["client_id"],
            new Uri(form["target_link_uri"]),
            form["lti_message_hint"],
            form["lti_storage_target"]
        );

        bindingContext.Result = ModelBindingResult.Success(launchRequest);

        return Task.CompletedTask;
    }
}