using System.Net;
using BFF.Web.Domain;
using BFF.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Refit;

namespace BFF.Web.API;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class PersonController : ControllerBase
{
    #region Constants
    private const string PERSON_ID_CLAIM_NAME = "personid";
    #endregion

    private readonly IEgressApi _egressApi;
    private readonly IAuthApi _authApi;

    public PersonController(IEgressApi egressApi, IAuthApi authApi)
    {
        _egressApi = egressApi;
        _authApi = authApi;
    }
    
    [HttpPut]
    [Microsoft.AspNetCore.Authorization.Authorize]
    [Route("register/profile-image")]
    public async Task<IActionResult> UpdateProfileImageAsync([FromForm(Name = "perfil_image")] IFormFile profileImage)
    {
        var personId = HttpContext.User.Claims.Single(c => c.Type.Equals(PERSON_ID_CLAIM_NAME)).Value;

        await using var stream = profileImage.OpenReadStream();
        var streamPart = new StreamPart(stream, profileImage.FileName, profileImage.ContentType);

        var response = await _egressApi.UpdateProfileImageAsync(personId, streamPart);

        return await HandleResponseAsync<object>(response);
    }

    [HttpGet]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public async Task<IActionResult> GetPersonInfoAsync()
    {
        var personId = HttpContext.User.Claims.Single(c => c.Type.Equals(PERSON_ID_CLAIM_NAME)).Value;

        var response = await _egressApi.GetPersonByIdAsync(personId);

        return await HandleResponseAsync<object>(response);
    }

    [HttpPost]
    [Route("register")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public async Task<IActionResult> RegisterPersonAsync([FromBody] RegisterPersonEntryModel request)
    {
        var accessToken = HttpContext.Request.Headers["Authorization"].Single()!;
        
        var (statusCode, userResponse) = await HandleExternalRequestAsync<UserResponse>(async () => 
            await _authApi.GetUserInfoAsync(accessToken));

        if (!statusCode.Equals(HttpStatusCode.OK)) return StatusCode((int)statusCode, userResponse);

        var user = userResponse.Data!;
        CompleteRegisterPersonWithUserResponse(request, user);
        
        (statusCode, var personResponse) = request.PersonType.Equals(PersonType.EGRESS)?
            await HandleExternalRequestAsync<RegisterPersonApiResponse>(async () => await _egressApi.UpdatePersonAsync(request)) 
            : await HandleExternalRequestAsync<RegisterPersonApiResponse>(async () => await _egressApi.RegisterPersonAsync(request));

        if (statusCode.Equals(HttpStatusCode.OK))
        {
            var personId = personResponse.Data!.PersonId.ToString();
            await _authApi.AssignPersonToRegistrationAsync(accessToken!, personId);
        }
        
        return StatusCode((int)statusCode, personResponse);
    }
    
    [HttpPut]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public async Task<IActionResult> UpdatePersonAsync([FromBody] RegisterPersonEntryModel request)
    {
        var accessToken = HttpContext.Request.Headers["Authorization"].Single()!;
        
        var (statusCode, userResponse) = await HandleExternalRequestAsync<UserResponse>(async () => 
            await _authApi.GetUserInfoAsync(accessToken));

        if (!statusCode.Equals(HttpStatusCode.OK)) return StatusCode((int)statusCode, userResponse);

        var user = userResponse.Data!;
        CompleteRegisterPersonWithUserResponse(request, user);

        var response = await _egressApi.UpdatePersonAsync(request);
        
        return await HandleResponseAsync<object>(response);
    }
    
    /// <summary>
    /// Complete request with user infos
    /// </summary>
    /// <param name="request">Request data</param>
    /// <param name="user">User info</param>
    private static void CompleteRegisterPersonWithUserResponse(RegisterPersonEntryModel request, UserResponse user)
    {
        request.Id = user.PersonId;
        request.Cpf = user.DocumentType.Equals(DocumentType.Cpf)? user.Document : request.Cpf;
        request.Name = user.Name;
        request.Email = user.Email;
        request.PersonType = Enum.Parse<PersonType>(user.UserType.ToString(), true);
    }
    
    /// <summary>
    /// Call external request and handle data
    /// </summary>
    /// <param name="externalRequestAsync">External request function</param>
    /// <returns>Status code and GenericHttpResponse</returns>
    private static async Task<(HttpStatusCode, GenericHttpResponse<T>)> HandleExternalRequestAsync<T>(
        Func<Task<HttpResponseMessage>> externalRequestAsync) where T : class
    {
        var response = await externalRequestAsync();
        var responseBody = await response.Content.ReadAsStringAsync();
        var deserializedResponseBody = JsonConvert.DeserializeObject<GenericHttpResponse<T>>(responseBody);
        return (response.StatusCode, deserializedResponseBody!);
    }
    
    /// <summary>
    /// Read body message and build response
    /// </summary>
    /// <param name="httpResponseMessage">httpResponseMessage</param>
    /// <returns>IActionResult with returned status code and body</returns>
    private async Task<IActionResult> HandleResponseAsync<T>(HttpResponseMessage httpResponseMessage) where T : class
    {
        var responseBody = await httpResponseMessage.Content.ReadAsStringAsync();

        var deserializedBody = responseBody.DeserializeOrDefault<GenericHttpResponse<T>>();
        
        return StatusCode((int)httpResponseMessage.StatusCode, deserializedBody);
    }
}
