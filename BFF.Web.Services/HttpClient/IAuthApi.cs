using BFF.Web.Domain;
using Refit;

namespace BFF.Web.Services;

public interface IAuthApi
{
    #region Constants
    private const string SEND_EMAIL_RESET_PASSWORD_HEADER_EMAIL = "email";
    private const string CONFIRM_ACCOUNT_HEADER_EMAIL = "email";
    private const string CONFIRM_ACCOUNT_HEADER_TOKEN = "token";
    private const string REFRESH_TOKEN_HEADER = "refresh_token";
    private const string AUTHORIZATION_HEADER = "Authorization";
    private const string PERSON_ID_HEADER = "person_id";
    private const string PAGE_NUMBER = "page_number";
    private const string PAGE_SIZE = "page_size";
    private const string ID_PARAM = "id";
    #endregion

    [Post("/api/v1/user/register")]
    Task<HttpResponseMessage> RegisterAsync([Body] RegisterRequestApi request);

    [Post("/api/v1/user/authenticate")]
    Task<HttpResponseMessage> AuthenticationAsync([Body] AuthenticationRequestApi request);
    
    [Post("/api/v1/user/password_reset/send")]
    Task<HttpResponseMessage> SendEmailResetPasswordAsync([Header(SEND_EMAIL_RESET_PASSWORD_HEADER_EMAIL)] string email);

    [Post("/api/v1/user/password_reset")]
    Task<HttpResponseMessage> ResetPasswordAsync([Body] ResetPasswordRequestApi request);

    [Get("/api/v1/user/confirm_account")]
    Task<HttpResponseMessage> ConfirmAccountAsync([AliasAs(CONFIRM_ACCOUNT_HEADER_TOKEN)] string token, [AliasAs(CONFIRM_ACCOUNT_HEADER_EMAIL)] string email);

    [Post("/api/v1/user/refreshtoken")]
    Task<HttpResponseMessage> RefreshTokenAsync([Header(REFRESH_TOKEN_HEADER)] string refreshToken);
    
    [Post("/api/v1/user/change_password")]
    Task<HttpResponseMessage> ChangePasswordAsync([Header(AUTHORIZATION_HEADER)] string authorization, [Body] ChangePasswordRequestApi request);
    
    [Get("/api/v1/user")]
    Task<HttpResponseMessage> GetUserInfoAsync([Header(AUTHORIZATION_HEADER)] string authorization);

    [Post("/api/v1/contact")]
    Task<HttpResponseMessage> SendContactEmailAsync([Body] ContactEmailRequestApi request);

    [Put("/api/v1/user/assign-person")]
    Task<HttpResponseMessage> AssignPersonToRegistrationAsync([Header(AUTHORIZATION_HEADER)] string authorization, [Header(PERSON_ID_HEADER)] string personId);
    
    [Get("/api/v1/user/lockout/all")]
    Task<HttpResponseMessage> GetPaginateLockoutUsersAsync(
        [Header(AUTHORIZATION_HEADER)] string authorization,
        [AliasAs(PAGE_NUMBER)] int pageNumber,
        [AliasAs(PAGE_SIZE)] int pageSize);
    
    [Put("/api/v1/user/unlock/{id}")]
    Task<HttpResponseMessage> UnlockUserAsync(
        [Header(AUTHORIZATION_HEADER)] string authorization,
        [AliasAs(ID_PARAM)] string id);
}
