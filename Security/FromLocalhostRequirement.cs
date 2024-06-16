using Microsoft.AspNetCore.Authorization;

namespace Security;

public class FromLocalhostRequirement : IAuthorizationRequirement {
}

public class FromLocalhostAttribute : AuthorizeAttribute, IAuthorizationRequirementData
{
    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        return [new FromLocalhostRequirement()];
    }
}

public class FromLocalhostHandler(IHttpContextAccessor httpContext, ILogger<FromLocalhostHandler> logger) : AuthorizationHandler<FromLocalhostRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, FromLocalhostRequirement requirement)
    {
        var connection = httpContext.HttpContext?.Connection;
        if(connection == null)
        {
            logger.LogWarning("No connection found");
            context.Fail();
            return Task.CompletedTask;
        }
        var remoteAddr = connection.RemoteIpAddress;
        if (remoteAddr == null)
        {
            logger.LogWarning("No remote address found");
            context.Fail();
            return Task.CompletedTask;
        }
        if (!remoteAddr.ToString().Equals("::1") && !remoteAddr.ToString().Equals("127.0.0.1"))
        {
            logger.LogInformation("Remote address is not localhost");
            context.Fail();
            return Task.CompletedTask;
        }
        logger.LogDebug("Remote address is localhost");
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
