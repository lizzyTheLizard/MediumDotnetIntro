using System.Security.Principal;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Security;

public class CustomSchemeOptions : AuthenticationSchemeOptions
{
    public string? Realm { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }

}

public class CustomSchemeHandler(IOptionsMonitor<CustomSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : AuthenticationHandler<CustomSchemeOptions>(options, logger, encoder)
{
    protected override Task InitializeHandlerAsync()
    {
        if (Options.Username == null)
        {
            return Task.FromException(new ArgumentNullException(nameof(Options.Username)));
        }
        if (Options.Password == null)
        {
            return Task.FromException(new ArgumentNullException(nameof(Options.Password)));
        }
        if (Options.Realm == null)
        {
            return Task.FromException(new ArgumentNullException(nameof(Options.Realm)));
        }
        return Task.CompletedTask;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var headers = (from h in Request.Headers.Authorization
                       where h.StartsWith("Basic ", StringComparison.InvariantCultureIgnoreCase)
                       select h.Substring(6)).ToList();
        if(headers.Count == 0)
        {
            Logger.LogDebug("No Authentication Header, not authenticated");
            return Task.FromResult(AuthenticateResult.NoResult());
        }
        if (headers.Count > 1)
        {
            var message = "Multiple Authorization Headers, not supported";
            return Task.FromResult(AuthenticateResult.Fail(message));
        }
        var decodedBytes = Convert.FromBase64String(headers[0]);
        var decoced = System.Text.Encoding.UTF8.GetString(decodedBytes);
        var split = decoced.Split(':');
        if(split.Length != 2)
        {
            var message = "Authentication Header not valid";
            return Task.FromResult(AuthenticateResult.Fail(message));
        }
        var username = split[0];
        var password = split[1];
        if (!username.Equals(Options.Username))
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid username or password"));
        }
        if (!password.Equals(Options.Password))
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid username or password"));
        }
        var identity = new GenericIdentity(username);
        var principal = new GenericPrincipal(identity, ["user"]);
        var properties = new AuthenticationProperties();
        var ticket = new AuthenticationTicket(principal, properties, CustomSchemeDefaults.AuthenticationScheme);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        //Never send 401 => If there is no authentication header we use OpenIdConnect
        return Task.CompletedTask;
    }
}

public static class CustomSchemeDefaults
{
    public const string AuthenticationScheme = "CustomAuthenticationScheme";
}

public static class CustomSchemeExtensions
{
    public static AuthenticationBuilder AddCustom(this AuthenticationBuilder builder, Action<CustomSchemeOptions> options)
    {
        builder.AddScheme<CustomSchemeOptions, CustomSchemeHandler>(
            CustomSchemeDefaults.AuthenticationScheme, options);
        return builder;
    }
}
