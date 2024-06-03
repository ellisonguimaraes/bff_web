using Newtonsoft.Json;

namespace BFF.Web.Domain;

public class AuthenticationRequestApi
{
    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("password")]
    public string Password { get; set; }
}
