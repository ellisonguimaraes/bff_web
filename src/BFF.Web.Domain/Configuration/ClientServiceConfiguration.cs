using System.Text.Json.Serialization;

namespace BFF.Web.Domain;

public class ClientServiceConfiguration
{
    [JsonPropertyName("ApplicationName")]
    public string? ApplicationName { get; set; }
    
    [JsonPropertyName("Host")]
    public string? Host { get; set; }
}
