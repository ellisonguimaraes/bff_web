using Newtonsoft.Json;

namespace BFF.Web.Domain.GetPersonById;

public sealed class GetPersonByIdHighlightsApiResponse : GetPersonByIdBaseApiResponse
{
    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("link")]
    public string? Link { get; set; }

    [JsonProperty("was_accepted")]
    public bool WasAccepted { get; set; }
}