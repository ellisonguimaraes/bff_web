using Newtonsoft.Json;

namespace BFF.Web.Domain.GetPersonById;

public abstract class GetPersonByIdBaseApiResponse
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }
}