using BFF.Web.Domain;
using BFF.Web.Domain.HttpClient.Highlights;

namespace BFF.Web.Services;

public interface IHttpClientEgressApi
{
    Task<HttpResponseMessage> RequestHighlightsAsync(RequestForHighlightsRequest request);
}