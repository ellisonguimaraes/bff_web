using Newtonsoft.Json;

namespace BFF.Web.Domain.HttpClient.Testimony;

public sealed class GetPaginateTestimonyApiResponse
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    
    [JsonProperty("content")]
    public string Content { get; set; }

    [JsonProperty("was_accepted")]
    public bool WasAccepted { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("perfil_image_src")]
    public string? PerfilImageSrc { get; set; }

    [JsonProperty("courses")]
    public IEnumerable<string> Courses { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
