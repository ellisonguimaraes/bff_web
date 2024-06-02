using Newtonsoft.Json;

namespace BFF.Web.Domain.GetPersonById;

public sealed class GetPersonByIdEmploymentApiResponse : GetPersonByIdBaseApiResponse
{
    [JsonProperty("role")]
    public string? Role { get; set; }

    [JsonProperty("enterprise")]
    public string? Enterprise { get; set; }

    [JsonProperty("salary_range")]
    public decimal? SalaryRange { get; set; }

    [JsonProperty("is_public_initiative")]
    public bool? IsPublicInitiative { get; set; }
    
    [JsonProperty("is_public")] 
    public bool? IsPublic { get; set; }

    [JsonProperty("start_date")]
    public string? StartDate { get; set; }
}