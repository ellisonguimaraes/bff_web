using BFF.Web.Domain;
using BFF.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace BFF.Web.API;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Microsoft.AspNetCore.Authorization.Authorize(Roles = "ADM")]
public class AdminController : ControllerBase
{
    #region Constants
    private const string X_PAGINATION_HEADER = "X-Pagination";
    private const string GET_PAGINATE_TESTIMONIES_ORDER_BY = "CreatedAt desc";
    #endregion
    
    private readonly IEgressApi _egressApi;
    private readonly IAuthApi _authApi;

    public AdminController(IEgressApi egressApi, IAuthApi authApi)
    {
        _egressApi = egressApi;
        _authApi = authApi;
    }

    [HttpPost]
    [Route("person/create-person")]
    public async Task<IActionResult> CreateBasicPersonAsync([FromBody] CreateBasicPersonRequestApi request)
    {
        var response = await _egressApi.CreateBasicPersonAsync(request);
        return await HandleResponseAsync(response);
    }
    
    [HttpPost]
    [Route("person/create-person/batch")]
    public async Task<IActionResult> CreateBasicPersonBatchAsync([FromForm(Name = "batch")] IFormFile file)
    {
        await using var stream = file.OpenReadStream();
        var streamPart = new StreamPart(stream, file.FileName, file.ContentType);
        
        var response = await _egressApi.CreateBasicPersonBatchAsync(streamPart);
        
        return await HandleResponseAsync(response);
    }
    
    [HttpGet]
    [Route("person/{id}")]
    public async Task<IActionResult> GetPersonByIdAsync([FromRoute] string id)
    {
        var response = await _egressApi.GetPersonByIdAsync(id);
        return await HandleResponseAsync(response);
    }
    
    [HttpDelete]
    [Route("person/{id}")]
    public async Task<IActionResult> DeletePersonAsync([FromRoute] string id)
    {
        var response = await _egressApi.DeletePersonAsync(id);
        return await HandleResponseAsync(response);
    }
    
    [HttpGet]
    [Route("person")]
    public async Task<IActionResult> GetPaginatePersonAsync(
        [FromQuery(Name = "page_number")] int pageNumber,
        [FromQuery(Name = "page_size")] int pageSize,
        [FromQuery(Name = "query")] string query,
        [FromQuery(Name = "order_by")] string orderByProperty)
    {
        var response = await _egressApi.GetPaginatePersonAsync(pageNumber, pageSize, query, orderByProperty);
        
        var paginationHeader = response.Headers.FirstOrDefault(h => h.Key.Equals(X_PAGINATION_HEADER, StringComparison.OrdinalIgnoreCase));
        
        Response.Headers.Add(paginationHeader.Key, paginationHeader.Value.FirstOrDefault());

        return await HandleResponseAsync(response);
    }
    
    [HttpPut]
    [Route("highlights/accept/{id}")]
    public async Task<IActionResult> AcceptHighlightsAsync([FromRoute] string id)
    {
        var response = await _egressApi.AcceptHighlightsAsync(id);
        return await HandleResponseAsync(response);
    }
    
    [HttpDelete]
    [Route("highlights/{id}")]
    public async Task<IActionResult> DeleteHighlightsAsync([FromRoute] string id)
    {
        var response = await _egressApi.DeleteHighlightsAsync(id);
        return await HandleResponseAsync(response);
    }
    
    [HttpDelete]
    [Route("testimony/{id}")]
    public async Task<IActionResult> DeleteTestimonyAsync([FromRoute] string id)
    {
        var response = await _egressApi.DeleteTestimonyAsync(id);
        return await HandleResponseAsync(response);
    }
    
    [HttpPut]
    [Route("testimony/accept/{id}")]
    public async Task<IActionResult> AcceptTestimonyAsync([FromRoute] string id)
    {
        var response = await _egressApi.AcceptTestimonyAsync(id);
        return await HandleResponseAsync(response);
    }
    
    [HttpGet]
    [Route("user/lockout")]
    public async Task<IActionResult> GetLockoutUsersAsync(
        [FromQuery(Name = "page_number")] int pageNumber,
        [FromQuery(Name = "page_size")] int pageSize)
    {
        var accessToken = HttpContext.Request.Headers["Authorization"];
        
        var response = await _authApi.GetPaginateLockoutUsersAsync(accessToken!, pageNumber, pageSize);
        
        var paginationHeader = response.Headers.FirstOrDefault(h => h.Key.Equals(X_PAGINATION_HEADER, StringComparison.OrdinalIgnoreCase));
        Response.Headers.Add(paginationHeader.Key, paginationHeader.Value.FirstOrDefault());

        return await HandleResponseAsync(response);
    }
    
    [HttpPut]
    [Route("user/unlock/{id}")]
    public async Task<IActionResult> UnlockUserAsync([FromRoute] string id)
    {
        var accessToken = HttpContext.Request.Headers["Authorization"];
        var response = await _authApi.UnlockUserAsync(accessToken!, id);
        return await HandleResponseAsync(response);
    }
    
    [HttpGet]
    [Route("testimony")]
    public async Task<IActionResult> GetPaginateTestimoniesAsync(
        [FromQuery(Name = "page_number")] int pageNumber,
        [FromQuery(Name = "page_size")] int pageSize)
    {
        var response = await _egressApi.GetPaginateTestimoniesAsync(pageNumber, pageSize,
            string.Empty, GET_PAGINATE_TESTIMONIES_ORDER_BY);

        var paginationHeader =
            response.Headers.FirstOrDefault(h => h.Key.Equals(X_PAGINATION_HEADER, StringComparison.OrdinalIgnoreCase));

        Response.Headers.Add(paginationHeader.Key, paginationHeader.Value.FirstOrDefault());

        return await HandleResponseAsync(response);
    }
    
    [HttpGet]
    [Route("highlights")]
    public async Task<IActionResult> GetPaginateHighlightsAsync(
        [FromQuery(Name = "page_number")] int pageNumber,
        [FromQuery(Name = "page_size")] int pageSize)
    {
        var response = await _egressApi.GetPaginateHighlightsAsync(pageNumber, pageSize,
            string.Empty, GET_PAGINATE_TESTIMONIES_ORDER_BY);

        var paginationHeader =
            response.Headers.FirstOrDefault(h => h.Key.Equals(X_PAGINATION_HEADER, StringComparison.OrdinalIgnoreCase));

        Response.Headers.Add(paginationHeader.Key, paginationHeader.Value.FirstOrDefault());

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