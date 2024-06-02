using Newtonsoft.Json;

namespace BFF.Web.Domain.GetPersonById;

public sealed class GetPersonByIdContinuingEducationApiResponse : GetPersonByIdBaseApiResponse
{
    [JsonProperty("is_public")]
    public bool IsPublic { get; set; }
    
    [JsonProperty("has_certification")]
    public bool HasCertification { get; set; }
    
    [JsonProperty("has_specialization")]
    public bool HasSpecialization { get; set; }
    
    [JsonProperty("has_master_degree")]
    public bool HasMasterDegree { get; set; }
    
    [JsonProperty("has_doctorate_degree")]
    public bool HasDoctorateDegree { get; set; }
}