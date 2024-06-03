using Newtonsoft.Json;

namespace BFF.Web.Domain;

public class ChangePasswordRequestApi
{
    [JsonProperty("password")]
    public string Password { get; set; }

    [JsonProperty("new_password")]
    public string NewPassword { get; set; }

    [JsonProperty("new_password_repeat")]
    public string NewPasswordRepeat { get; set; }
}
