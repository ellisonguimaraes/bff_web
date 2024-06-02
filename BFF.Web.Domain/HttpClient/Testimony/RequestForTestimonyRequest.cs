using Newtonsoft.Json;

namespace BFF.Web.Domain.HttpClient.Testimony;

public class RequestForTestimonyRequest
{
    [JsonProperty("content")]
    public string Content { get; set; }
}