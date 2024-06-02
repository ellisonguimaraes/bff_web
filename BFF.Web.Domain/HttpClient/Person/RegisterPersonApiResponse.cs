using Newtonsoft.Json;

namespace BFF.Web.Domain;

public sealed class RegisterPersonApiResponse
{
    [JsonProperty("person_id")]
    public Guid PersonId { get; set; }
}
