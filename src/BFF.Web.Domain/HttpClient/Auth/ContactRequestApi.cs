using Newtonsoft.Json;

namespace BFF.Web.Domain;

public class ContactEmailRequestApi
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("subject")]
    public string Subject { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("phone_number")]
    public string PhoneNumber { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }
}