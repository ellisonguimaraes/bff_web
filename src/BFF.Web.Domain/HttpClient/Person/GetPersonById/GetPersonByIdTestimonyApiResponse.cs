using Newtonsoft.Json;

namespace BFF.Web.Domain.GetPersonById;

public sealed class GetPersonByIdTestimonyApiResponse : GetPersonByIdBaseApiResponse
{
    [JsonProperty("content")]
    public string Content { get; set; }

    [JsonProperty("was_accepted")]
    public bool WasAccepted { get; set; }
}