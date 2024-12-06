using System.Net;
using BFF.Web.Domain;
using BFF.Web.Domain.GetPersonById;
using BFF.Web.Domain.HttpClient.Highlights;
using BFF.Web.Domain.HttpClient.Testimony;
using BFF.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace BFF.Web.API;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class EgressController : ControllerBase
{
    #region Constants
    private const string X_PAGINATION_HEADER = "X-Pagination";
    private const string GET_PAGINATE_TESTIMONIES_QUERY_STRING = "WasAccepted equal true";
    private const string GET_PAGINATE_TESTIMONIES_ORDER_BY = "CreatedAt desc";
    private const string GET_HIGHLIGHTS_QUERY_STRING = "WasAccepted equal true and Id equal \"{0}\"";
    private const int GET_HIGHLIGHTS_PAGE_SIZE = 1;
    private const int GET_HIGHLIGHTS_PAGE_NUMBER = 1;
    private const string PERSON_ID_CLAIM_NAME = "personid";
    private const string DELETE_TESTIMONY_QUERY = "Person.Id equal \"{0}\" && Id equal \"{1}\"";
    private const string DELETE_HIGHLIGHT_QUERY = "Person.Id equal \"{0}\" && Id equal \"{1}\"";

    #endregion

    private readonly IEgressApi _egressApi;
    private readonly IHttpClientEgressApi _httpClientEgressApi;

    public EgressController(IEgressApi egressApi, IHttpClientEgressApi httpClientEgressApi)
    {
        _egressApi = egressApi;
        _httpClientEgressApi = httpClientEgressApi;
    }

    [HttpGet]
    [Route("highlights/random/{quantity:int}")]
    public async Task<IActionResult> GetRandomHighlightsAsync(int quantity)
    {
        var response = await _egressApi.GetRandomHighlightsAsync(quantity);
        return await HandleResponseAsync<object>(response);
    }

    [HttpGet]
    [Route("highlights")]
    public async Task<IActionResult> GetPaginateHighlightsAsync(
        [FromQuery(Name = "page_number")] int pageNumber,
        [FromQuery(Name = "page_size")] int pageSize)
    {
        var response = await _egressApi.GetPaginateHighlightsAsync(pageNumber, pageSize,
            GET_PAGINATE_TESTIMONIES_QUERY_STRING, GET_PAGINATE_TESTIMONIES_ORDER_BY);

        var paginationHeader =
            response.Headers.FirstOrDefault(h => h.Key.Equals(X_PAGINATION_HEADER, StringComparison.OrdinalIgnoreCase));

        Response.Headers.Add(paginationHeader.Key, paginationHeader.Value.FirstOrDefault());

        return await HandleResponseAsync<object>(response);
    }

    [HttpGet]
    [Route("highlights/{id:Guid}")]
    public async Task<IActionResult> GetHighlightsAsync([FromRoute(Name = "id")] Guid id)
    {
        var response = await _egressApi.GetPaginateHighlightsAsync(GET_HIGHLIGHTS_PAGE_NUMBER, GET_HIGHLIGHTS_PAGE_SIZE,
            string.Format(GET_HIGHLIGHTS_QUERY_STRING, id.ToString()), GET_PAGINATE_TESTIMONIES_ORDER_BY);
        return await HandleResponseAsync<object>(response);
    }

    [HttpPost]
    [Route("highlights")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public async Task<IActionResult> RequestHighlightsAsync([FromForm] RequestForHighlightsRequest request)
    {
        var personId = HttpContext.User.Claims.Single(c => c.Type.Equals(PERSON_ID_CLAIM_NAME)).Value;

        request.PersonId = personId;

        var response = await _httpClientEgressApi.RequestHighlightsAsync(request);

        return await HandleResponseAsync<object>(response);
    }

    [HttpPost]
    [Route("testimony")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public async Task<IActionResult> RequestTestimonyAsync([FromBody] RequestForTestimonyRequest request)
    {
        var personId = HttpContext.User.Claims.Single(c => c.Type.Equals(PERSON_ID_CLAIM_NAME)).Value;
        var response = await _egressApi.RequestTestimonyAsync(personId, request);
        return await HandleResponseAsync<object>(response);
    }

    [HttpGet]
    [Route("testimony/random/{quantity:int}")]
    public async Task<IActionResult> GetRandomTestimoniesAsync(int quantity)
    {
        var response = await _egressApi.GetRandomTestimoniesAsync(quantity);
        return await HandleResponseAsync<object>(response);
    }

    [HttpGet]
    [Route("testimony")]
    public async Task<IActionResult> GetPaginateTestimoniesAsync(
        [FromQuery(Name = "page_number")] int pageNumber,
        [FromQuery(Name = "page_size")] int pageSize)
    {
        var response = await _egressApi.GetPaginateTestimoniesAsync(pageNumber, pageSize,
            GET_PAGINATE_TESTIMONIES_QUERY_STRING, GET_PAGINATE_TESTIMONIES_ORDER_BY);

        var paginationHeader =
            response.Headers.FirstOrDefault(h => h.Key.Equals(X_PAGINATION_HEADER, StringComparison.OrdinalIgnoreCase));

        Response.Headers.Add(paginationHeader.Key, paginationHeader.Value.FirstOrDefault());

        return await HandleResponseAsync<object>(response);
    }

    [HttpGet]
    [Route("per-year")]
    public async Task<IActionResult> GetCountEgressPerFinalSemesterAsync()
    {
        var response = await _egressApi.GetCountEgressPerFinalSemesterAsync();
        return await HandleResponseAsync<object>(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetPaginateEgressAsync(
        [FromQuery(Name = "page_number")] int pageNumber,
        [FromQuery(Name = "page_size")] int pageSize,
        [FromQuery(Name = "query")] string query,
        [FromQuery(Name = "order_by")] string orderByProperty)
    {
        var response = await _egressApi.GetPaginateEgressAsync(pageNumber, pageSize, query, orderByProperty);

        var paginationHeader =
            response.Headers.FirstOrDefault(h => h.Key.Equals(X_PAGINATION_HEADER, StringComparison.OrdinalIgnoreCase));

        Response.Headers.Add(paginationHeader.Key, paginationHeader.Value.FirstOrDefault());

        return await HandleResponseAsync<object>(response);

    }

    [HttpDelete]
    [Route("testimony/{id}")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public async Task<IActionResult> DeleteTestimonyAsync([FromRoute(Name = "id")] Guid id)
    {
        var personId = HttpContext.User.Claims.Single(c => c.Type.Equals(PERSON_ID_CLAIM_NAME)).Value;

        var query = string.Format(DELETE_TESTIMONY_QUERY, personId, id);

        var result = await _egressApi.GetPaginateTestimoniesAsync(1, 1, query, string.Empty);

        var responseBody = await result.Content.ReadAsStringAsync();
        var deserializedBody = responseBody.DeserializeOrDefault<GenericHttpResponse<IList<GetPaginateTestimonyApiResponse>>>();

        if (!result.IsSuccessStatusCode)
            return StatusCode((int)result.StatusCode, deserializedBody);
        
        var testimonyId = deserializedBody?.Data?.FirstOrDefault()?.Id ?? new Guid();
        result = await _egressApi.DeleteTestimonyAsync(testimonyId.ToString());
        
        return await HandleResponseAsync<object>(result);
    }
    
    [HttpDelete]
    [Route("highlights/{id}")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public async Task<IActionResult> DeleteHighlightsAsync([FromRoute(Name = "id")] Guid id)
    {
        var personId = HttpContext.User.Claims.Single(c => c.Type.Equals(PERSON_ID_CLAIM_NAME)).Value;

        var query = string.Format(DELETE_HIGHLIGHT_QUERY, personId, id);

        var result = await _egressApi.GetPaginateHighlightsAsync(1, 1, query, string.Empty);

        var responseBody = await result.Content.ReadAsStringAsync();
        var deserializedBody = responseBody.DeserializeOrDefault<GenericHttpResponse<IList<GetPaginateHighlightsApiResponse>>>();

        if (!result.IsSuccessStatusCode)
            return StatusCode((int)result.StatusCode, deserializedBody);
        
        var highlightId = deserializedBody?.Data?.FirstOrDefault()?.Id ?? new Guid();
        result = await _egressApi.DeleteHighlightsAsync(highlightId.ToString());
        
        return await HandleResponseAsync<object>(result);
    }
    
    [HttpGet]
    [Route("views")]
    public async Task<IActionResult> ChartsByViewAsync([FromHeader(Name = "views")] string views)
    {
        var response = await _egressApi.GetChartsByViewAsync(views);
        return await HandleResponseAsync<object>(response);
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
