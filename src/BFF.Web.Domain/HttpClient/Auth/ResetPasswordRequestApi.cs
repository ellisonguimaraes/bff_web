using Newtonsoft.Json;

namespace BFF.Web.Domain;

public class ResetPasswordRequestApi
{
    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("token")]
    public string Token { get; set; }

    [JsonProperty("new_password")]
    public string NewPassword { get; set; }
}
