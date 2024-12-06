using BFF.Web.Domain;
using BFF.Web.Domain.HttpClient.Note;
using BFF.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace BFF.Web.API;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Microsoft.AspNetCore.Authorization.Authorize]
public class NoteController : ControllerBase
{
    #region Constants
    private const string X_PAGINATION_HEADER = "X-Pagination";
    private const string GET_PAGINATE_NOTE_QUERY_STRING = "WasAccepted equal true";
    private const string GET_PAGINATE_NOTE_ORDER_BY = "CreatedAt desc";
    private const string PERSON_ID_CLAIM_NAME = "personid";
    private const string PAGE_NUMBER_QUERY_NAME = "page_number";
    private const string PAGE_SIZE_QUERY_NAME = "page_size";
    #endregion
    
    private readonly IEgressApi _egressApi;

    public NoteController(IEgressApi egressApi)
    {
        _egressApi = egressApi;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaginateNoteAsync(
        [FromQuery(Name = PAGE_NUMBER_QUERY_NAME)] int pageNumber,
        [FromQuery(Name = PAGE_SIZE_QUERY_NAME)] int pageSize)
    {
        var query = HttpContext.User.IsInRole("ADM")? string.Empty : GET_PAGINATE_NOTE_QUERY_STRING ;
        
        var response = await _egressApi.GetPaginateNotePersonAsync(pageNumber, pageSize, query, GET_PAGINATE_NOTE_ORDER_BY);

        var paginationHeader = response.Headers.FirstOrDefault(h => h.Key.Equals(X_PAGINATION_HEADER, StringComparison.OrdinalIgnoreCase));

        Response.Headers.Add(paginationHeader.Key, paginationHeader.Value.FirstOrDefault());

        return await HandleResponseAsync(response);
    }
    
    [HttpGet]
    [Route("my")]
    public async Task<IActionResult> GetPaginateMyNoteAsync(
        [FromQuery(Name = PAGE_NUMBER_QUERY_NAME)] int pageNumber,
        [FromQuery(Name = PAGE_SIZE_QUERY_NAME)] int pageSize)
    {
        var personId = HttpContext.User.Claims.Single(c => c.Type.Equals(PERSON_ID_CLAIM_NAME)).Value;

        var query = $"PersonId equal \"{personId}\"";
        
        var response = await _egressApi.GetPaginateNotePersonAsync(pageNumber, pageSize, query, GET_PAGINATE_NOTE_ORDER_BY);

        var paginationHeader = response.Headers.FirstOrDefault(h => h.Key.Equals(X_PAGINATION_HEADER, StringComparison.OrdinalIgnoreCase));

        Response.Headers.Add(paginationHeader.Key, paginationHeader.Value.FirstOrDefault());

        return await HandleResponseAsync(response);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetNoteByIdAsync([FromRoute(Name = "id")] Guid id)
    {
        var response = await _egressApi.GetNoteByIdAsync(id.ToString());
        return await HandleResponseAsync(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNoteAsync([FromBody] CreateNoteRequestApi request)
    {
        var personId = HttpContext.User.Claims.Single(c => c.Type.Equals(PERSON_ID_CLAIM_NAME)).Value;
        request.PersonId = personId;
        var response = await _egressApi.CreateNoteAsync(request);
        return await HandleResponseAsync(response);
    }
    
    [HttpPut]
    [Route("accept/{id:guid}")]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "ADM")]
    public async Task<IActionResult> AcceptNoteAsync([FromRoute(Name = "id")] Guid id)
    {
        var response = await _egressApi.AcceptNoteAsync(id.ToString());
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