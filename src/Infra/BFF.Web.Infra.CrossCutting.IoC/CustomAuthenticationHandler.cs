using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using BFF.Web.Domain;
using BFF.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BFF.Web.Infra.CrossCutting.IoC;

public class CustomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    #region Constants
    private const string PERSON_ID_CLAIM_NAME = "personid";
    private const string AUTHORIZATION_HEADER = "Authorization";
    private const string MISSING_AUTHORIZATION_HEADER_MESSAGE = "Missing Authorization Header";
    private const string AUTHORIZATION_API_REQUEST_UNSUCCESSFULLY_MESSAGE = "Authorization API request unsuccessfully";
    #endregion

    private readonly IAuthApi _authApi;

    public CustomAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IAuthApi authApi)
        : base(options, logger, encoder, clock)
    {
        _authApi = authApi;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        UserResponse user = null!;

        if (HasAllowAnonymous())
            return AuthenticateResult.NoResult();

        var authorizationHeader = GetAuthorizationHeader();

        if (authorizationHeader is null)
                return AuthenticateResult.Fail(MISSING_AUTHORIZATION_HEADER_MESSAGE);

        try
        {
            var response = await _authApi.GetUserInfoAsync($"{authorizationHeader.Scheme} {authorizationHeader.Parameter!}");
            response.EnsureSuccessStatusCode();
            var genericHttpResponse = JsonConvert.DeserializeObject<GenericHttpResponse<UserResponse>>(await response.Content.ReadAsStringAsync());
            user = genericHttpResponse!.Data!;
        }
        catch
        {
            return AuthenticateResult.Fail(AUTHORIZATION_API_REQUEST_UNSUCCESSFULLY_MESSAGE);
        }

        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Roles.FirstOrDefault() ?? string.Empty),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(PERSON_ID_CLAIM_NAME, user.PersonId.ToString() ?? string.Empty)
        };
        
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }

    private bool HasAllowAnonymous()
    {
        var endpoint = Context.GetEndpoint();
        return endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() is not null;
    }

    private AuthenticationHeaderValue? GetAuthorizationHeader()
    {
        try 
        {
            var authorizationHeader = Request.Headers[AUTHORIZATION_HEADER].FirstOrDefault();
            return !string.IsNullOrWhiteSpace(authorizationHeader)? AuthenticationHeaderValue.Parse(authorizationHeader) : default;
        } 
        catch
        {
            return default;
        } 
    }
}
