using BFF.Web.Domain;
using BFF.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace BFF.Web.API;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UserController : ControllerBase
{
    #region Constants
    private const string REFRESH_TOKEN_HEADER = "refresh_token";
    private const string TOKEN_QUERY_STRING = "token";
    private const string EMAIL_PARAMETER = "email";
    #endregion

    private readonly IAuthApi _authApi;

    public UserController(IAuthApi authApi)
    {
        _authApi = authApi;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequestApi request)
    {
        var response = await _authApi.RegisterAsync(request);
        return await HandleResponseAsync(response);
    }

    [HttpGet]
    [Route("confirm-account")]
    public async Task<IActionResult> ConfirmAccountAsync([FromQuery(Name = TOKEN_QUERY_STRING)] string token, [FromQuery(Name = EMAIL_PARAMETER)] string email)
    {
        var response = await _authApi.ConfirmAccountAsync(token, email);
        return await HandleResponseAsync(response);
    }
    
    [HttpPost]
    [Route("authenticate")]
    public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticationRequestApi request)
    {
        var response = await _authApi.AuthenticationAsync(request);
        return await HandleResponseAsync(response);
    }
    
    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync([FromHeader(Name = REFRESH_TOKEN_HEADER)] string refreshToken)
    {
        var response = await _authApi.RefreshTokenAsync(refreshToken);
        return await HandleResponseAsync(response);
    }

    [HttpPost]
    [Route("password-reset/send")]
    public async Task<IActionResult> SendEmailResetPasswordAsync([FromHeader(Name = EMAIL_PARAMETER)] string email)
    {
        var response = await _authApi.SendEmailResetPasswordAsync(email);
        return await HandleResponseAsync(response);
    }

    [HttpPost]
    [Route("password-reset")]
    public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequestApi request)
    {
        var response = await _authApi.ResetPasswordAsync(request);
        return await HandleResponseAsync(response);
    }
    
    [HttpPost]
    [Route("change-password")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequestApi request)
    {
        var accessToken = HttpContext.Request.Headers["Authorization"];
        
        var response = await _authApi.ChangePasswordAsync(accessToken!, request);
        
        return await HandleResponseAsync(response);
    }

    [HttpGet]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public async Task<IActionResult> GetUserInfoAsync()
    {
        var accessToken = HttpContext.Request.Headers["Authorization"];
        var response = await _authApi.GetUserInfoAsync(accessToken!);
        return await HandleResponseAsync(response);
    }
    
    /// <summary>
    /// Read body message and build response
    /// </summary>
    /// <param name="httpResponseMessage">httpResponseMessage</param>
    /// <returns>IActionResult with returned status code and body</returns>
    private async Task<IActionResult> HandleResponseAsync(HttpResponseMessage httpResponseMessage)
    {
        var responseBody = await httpResponseMessage.Content.ReadAsStringAsync();

        var deserializedBody = responseBody.DeserializeOrDefault<GenericHttpResponse<object>>();
        
        return StatusCode((int)httpResponseMessage.StatusCode, deserializedBody);
    }
}
