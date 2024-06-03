using Newtonsoft.Json;

namespace BFF.Web.Domain.HttpClient.Highlights;

public sealed class GetPaginateHighlightsApiResponse
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    
    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("link")]
    public string? Link { get; set; }

    [JsonProperty("was_accepted")]
    public bool WasAccepted { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("advertising_image_src")]
    public string? AdvertisingImageSrc { get; set; }

    [JsonProperty("veracity_files_src")]
    public IEnumerable<string> VeracityFilesSrc { get; set; }

    [JsonProperty("courses")]
    public IEnumerable<string> Courses { get; set; }

    [JsonProperty("perfil_image_src")]
    public string? PerfilImageSrc { get; set; }
    
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }
}