using Newtonsoft.Json;

namespace BFF.Web.Domain;

public class UserResponse
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("document")]
    public string Document { get; set; }

    [JsonProperty("document_type")]
    public DocumentType DocumentType { get; set; }

    [JsonProperty("person_id")]
    public Guid? PersonId { get; set; }

    [JsonProperty("user_type")]
    public UserType UserType { get; set; }

    [JsonProperty("email_confirmed")]
    public bool EmailConfirmed { get; set; }

    [JsonProperty("lockout_end")]
    public DateTimeOffset? LockoutEnd { get; set; }

    [JsonProperty("lockout_enabled")]
    public bool LockoutEnabled { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("edited_at")]
    public DateTime EditedAt { get; set; }

    [JsonProperty("roles")]
    public IEnumerable<string> Roles { get; set; }
}