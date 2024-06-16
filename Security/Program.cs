using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Security;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.ConfigureAuthentication();
        builder.ConfigureAuthorization();
        builder.Services.AddCors(o => o.AddPolicy(name: "AllowOrigins", p => p.WithOrigins("http://localhost:5000")));

        var app = builder.Build();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors("AllowOrigins");
        app.MapControllers();
        app.Run();
    }
}

file static class ProgramExtensions
{
    public static void ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        // The Cookie-Scheme sets a cookie to remember the user. The LoginPath is used to redirect the user to the login page.
        builder.Services.AddAuthentication(o =>
        {
            //The default authenticate is used when no other scheme is set in the policy. It is used even if no [Authorize] is set => But only for authorization
            //o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //The default challenge scheme is used if no scheme is specified in the profile and a challenge must be send (e.g. no authentication but needed)
            o.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            //The default forbid scheme is used if no scheme is specified in the profile and a forbid must be send (e.g. sucessfull authentication but not allowed)
            //o.DefaultForbidScheme = OpenIdConnectDefaults.AuthenticationScheme;
            // The default signin scheme is used if no scheme is specified in the profile and a signin is triggered.
            //o.DefaultSignInScheme = OpenIdConnectDefaults.AuthenticationScheme;
            // The default signout scheme is used if no scheme is specified in the profile and a signout is triggered.
            //o.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
            // The default scheme is used when no Default***Scheme is set
            o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
        //Add cookie authentication. This will "store" the authentication performed by another scheme in a cookie
        .AddCookie(o => o.ForwardAuthenticate = CustomSchemeDefaults.AuthenticationScheme)
        //Add a custom writen scheme (similar to basic auth)
        .AddCustom(o => {
            o.Username = "Admin";
            o.Password = builder.Configuration["ADMIN_PASSWORD"]; ;
            o.Realm = builder.Configuration[CustomSchemeDefaults.AuthenticationScheme + ":Realm"];
        })
        //Add OpenIdConnect (Microsoft) authentication
        .AddOpenIdConnect(o =>
        {
            o.Authority = "https://login.microsoftonline.com/" + builder.Configuration["APP_ID"];
            o.ClientId = builder.Configuration["CLIENT_ID"];
            o.ClientSecret = builder.Configuration["CLIENT_SECRET"];
        });
        /*
            * Alternatively use Microsoft directly
        })
        .AddMicrosoftAccount(o =>
        {
            o.ClientId = builder.Configuration["CLIENT_ID"];
            o.ClientSecret = builder.Configuration["CLIENT_SECRET"];
        });
        */
    }

    public static void ConfigureAuthorization(this WebApplicationBuilder builder)
    {
        // Define Authorization here. You can define multiple policies:
        // * The FallbackPolicy (used if nothing is defined on the controller)
        // * The DefaultPolicy (used if only [Authorize] is used on the controller)
        // * Named Policies (used if [Authorize(Policy = "Admin")] is used on the controller
        // Note: If you have [AllowAnonymous] on the controller, no policy is not used.
        //
        // For each policy you can define
        // * Requirements (any kind of requirement, must implement IAuthorizationRequirement and you have to define a handler)
        // * Assertions (simple kind of Requirement, handler is alreay defined)
        // * Roles (just strings)
        // * Claims (Key-Value-Pairs)
        // * User-Name (matcher) 
        // * AuthenticationSchemes (Only Users from this scheme are allowed)
        // Beside policies, you can also define Roles and Schemes directely on the Controller using [Authorize(Roles="Admin")]
        // or define your own AuthorizeAttribute like FromLocalhostAttribute that can be used like [FromLocalhost]
        var isAuthenticated = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
        var isAdmin = new AuthorizationPolicyBuilder()
            .RequireRole("Admin")
            .Build();
        var isLocal = new AuthorizationPolicyBuilder()
            .AddRequirements(new FromLocalhostRequirement())
            .RequireAssertion(context => true)
            .Build();
        builder.Services.AddAuthorizationBuilder()
            .SetFallbackPolicy(isAuthenticated)
            .SetDefaultPolicy(isAuthenticated)
            .AddPolicy("Local", isLocal)
            .AddPolicy("Admin", isAdmin);
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSingleton<IAuthorizationHandler, FromLocalhostHandler>();
        builder.Services.AddSingleton<IAuthorizationHandler, OnlyAdminSeesColdRequirementHandler>();
    }
}
