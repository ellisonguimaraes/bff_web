using BFF.Web.Domain;
using BFF.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace BFF.Web.API;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ChartsController : ControllerBase
{
    private readonly IEgressApi _egressApi;

    public ChartsController(IEgressApi egressApi)
    {
        _egressApi = egressApi;
    }


    [HttpGet]
    [Route("views")]
    public async Task<IActionResult> ChartsByViewAsync([FromHeader(Name = "views")] string views)
    {
        var response = await _egressApi.GetChartsByViewAsync(views);
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