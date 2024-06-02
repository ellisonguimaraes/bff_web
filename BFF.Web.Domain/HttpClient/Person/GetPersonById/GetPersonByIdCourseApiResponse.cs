using Newtonsoft.Json;

namespace BFF.Web.Domain.GetPersonById;

public sealed class GetPersonByIdCourseApiResponse : GetPersonByIdBaseApiResponse
{
    [JsonProperty("course_name")]
    public string CourseName { get; set; }
    
    [JsonProperty("beginning_semester")]
    public string BeginningSemester { get; set; }

    [JsonProperty("final_semester")]
    public string? FinalSemester { get; set; }

    [JsonProperty("mat")]
    public string Mat { get; set; }

    [JsonProperty("level")]
    public Level Level { get; set; }
    
    [JsonProperty("modality")]
    public Modality Modality { get; set; }
}