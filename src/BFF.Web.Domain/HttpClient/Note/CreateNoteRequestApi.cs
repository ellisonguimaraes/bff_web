using Newtonsoft.Json;

namespace BFF.Web.Domain.HttpClient.Note;

public sealed class CreateNoteRequestApi
{
    [JsonProperty("title")]
    public string Title { get; set; }
    
    [JsonProperty("content")]
    public string Content { get; set; }
    
    [JsonProperty("person_id")]
    public string PersonId { get; set; }
}