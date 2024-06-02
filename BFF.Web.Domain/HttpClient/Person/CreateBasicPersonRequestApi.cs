using Newtonsoft.Json;

namespace BFF.Web.Domain;

public class CreateBasicPersonRequestApi 
{
    [JsonProperty("id")]
    public Guid? Id { get; set; }

    [JsonProperty("cpf")]
    public string Cpf { get; set; }  = string.Empty;

    [JsonProperty("name")]
    public string Name { get; set; }  = string.Empty;

    [JsonProperty("birth_date")]
    public DateTime? BirthDate { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    [JsonProperty("phone_number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [JsonProperty("can_expose_data")]
    public bool? CanExposeData { get; set; }

    [JsonProperty("can_receive_message")]
    public bool? CanReceiveMessage { get; set; }

    [JsonProperty("person_type")]
    public PersonType? PersonType { get; set; }

    [JsonProperty("course")]
    public CourseRequestApi? Course { get; set; }
}

public class CourseRequestApi
{
    [JsonProperty("course_id")]
    public Guid CourseId { get; set; }

    [JsonProperty("beginning_semester")]
    public string BeginningSemester { get; set; } = string.Empty;

    [JsonProperty("final_semester")]
    public string FinalSemester { get; set; } = string.Empty;

    [JsonProperty("mat")]
    public string Mat { get; set; }  = string.Empty;

    [JsonProperty("level")]
    public Level Level { get; set; }

    [JsonProperty("modality")]
    public Modality Modality { get; set; }
}