using Newtonsoft.Json;

namespace BFF.Web.Domain;

public class RegisterRequestApi
{
    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("password")]
    public string Password { get; set; }

    [JsonProperty("confirm_password")]
    public string PasswordRepeat { get; set; }

    [JsonProperty("document")]
    public string Document { get; set; }

    [JsonProperty("document_type")]
    public DocumentType DocumentType { get; set; }

    [JsonProperty("user_type")]
    public UserType UserType { get; set; }
}
