using BFF.Web.Domain;
using BFF.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace BFF.Web.API;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public sealed class CourseController : ControllerBase
{
    private readonly IEgressApi _egressApi;

    public CourseController(IEgressApi egressApi)
    {
        _egressApi = egressApi;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCoursesAsync()
    {
        var response = await _egressApi.GetAllCoursesAsync();
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