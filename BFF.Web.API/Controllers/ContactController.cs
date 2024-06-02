using BFF.Web.Domain;
using BFF.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace BFF.Web.API;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ContactController : ControllerBase
{
    private readonly IAuthApi _authApi;

    public ContactController(IAuthApi authApi)
    {
        _authApi = authApi;
    }
    
    /// <summary>
    /// Send contact email to collegiate
    /// </summary>
    /// <param name="request">Email data</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> SendContactEmailAsync([FromBody]ContactEmailRequestApi request)
    {
        var response = await _authApi.SendContactEmailAsync(request);
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
