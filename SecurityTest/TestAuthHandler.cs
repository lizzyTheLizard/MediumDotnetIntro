using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Security;

using System.Security.Principal;
using System.Text.Encodings.Web;

namespace SecurityTest;

public class TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var identity = new GenericIdentity("Test");
        var principal = new GenericPrincipal(identity, ["user"]);
        var properties = new AuthenticationProperties();
        var ticket = new AuthenticationTicket(principal, properties, CustomSchemeDefaults.AuthenticationScheme);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
