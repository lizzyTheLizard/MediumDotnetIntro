using Microsoft.AspNetCore.Authorization;

namespace Security;

public class OnlyAdminSeesColdRequirement : IAuthorizationRequirement
{
}

public class OnlyAdminSeesColdRequirementHandler : AuthorizationHandler<OnlyAdminSeesColdRequirement, WeatherForecast>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OnlyAdminSeesColdRequirement requirement, WeatherForecast resource)
    {
        if (context.User.Identity?.Name == "Admin")
        {
            context.Succeed(requirement);
        }
        if(resource.TemperatureC > 10)
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}

