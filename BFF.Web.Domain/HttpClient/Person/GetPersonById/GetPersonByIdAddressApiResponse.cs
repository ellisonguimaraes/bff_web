using Newtonsoft.Json;

namespace BFF.Web.Domain.GetPersonById;

public sealed class GetPersonByIdAddressApiResponse : GetPersonByIdBaseApiResponse
{
    [JsonProperty("state")]
    public string? State { get; set; }

    [JsonProperty("country")]
    public string? Country { get; set; }
    
    [JsonProperty("is_public")] 
    public bool? IsPublic { get; set; }
}