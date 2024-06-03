using BFF.Web.Domain.HttpClient.Highlights;

namespace BFF.Web.Services.Implementations;

public class HttpClientEgressApi : IHttpClientEgressApi
{
    #region Constants
    private const string EGRESS_API_CLIENT_NAME = "egress_api";
    private const string CONTENT_TYPE_HEADER = "Content-Type";
    private const string PERSON_ID_HEADER = "Person-Id";
    
    private const string REQUEST_HIGHLIGHTS_ROUTE = "api/v1/highlights/request";
    private const string REQUEST_HIGHLIGHTS_PARAM_TITLE = "title";
    private const string REQUEST_HIGHLIGHTS_PARAM_DESCRIPTION = "description";
    private const string REQUEST_HIGHLIGHTS_PARAM_LINK = "link";
    private const string REQUEST_HIGHLIGHTS_PARAM_ADVERTISING_IMAGE = "advertising_image";
    private const string REQUEST_HIGHLIGHTS_PARAM_VERACITY_FILES = "veracity_files";
    #endregion

    private readonly HttpClient _client;
    
    public HttpClientEgressApi(IHttpClientFactory factory)
    {
        _client = factory.CreateClient(EGRESS_API_CLIENT_NAME);
    }
    
    public async Task<HttpResponseMessage> RequestHighlightsAsync(RequestForHighlightsRequest request)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, REQUEST_HIGHLIGHTS_ROUTE);
        
        using var content = new MultipartFormDataContent();
        
        if (request.Title is not null) content.Add(new StringContent(request.Title), REQUEST_HIGHLIGHTS_PARAM_TITLE);
        if (request.Description is not null) content.Add(new StringContent(request.Description), REQUEST_HIGHLIGHTS_PARAM_DESCRIPTION);
        if (request.Link is not null) content.Add(new StringContent(request.Link), REQUEST_HIGHLIGHTS_PARAM_LINK);

        if (request.AdvertisingImage is not null)
        {
            var streamContent = new StreamContent(request.AdvertisingImage.OpenReadStream());
            streamContent.Headers.Add(CONTENT_TYPE_HEADER, request.AdvertisingImage.ContentType);
            content.Add(streamContent, REQUEST_HIGHLIGHTS_PARAM_ADVERTISING_IMAGE, request.AdvertisingImage.FileName);
        }

        if (request.VeracityFiles is not null)
            foreach (var file in request.VeracityFiles)
            {
                var streamContent = new StreamContent(file.OpenReadStream());
                streamContent.Headers.Add(CONTENT_TYPE_HEADER, file.ContentType);
                content.Add(streamContent, REQUEST_HIGHLIGHTS_PARAM_VERACITY_FILES, file.FileName);
            }

        httpRequestMessage.Content = content;
        httpRequestMessage.Headers.Add(PERSON_ID_HEADER, request.PersonId);
        
        var response = await _client.SendAsync(httpRequestMessage);

        return response;
    }
}