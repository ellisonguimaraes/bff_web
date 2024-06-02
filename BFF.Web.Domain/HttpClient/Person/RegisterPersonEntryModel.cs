using Newtonsoft.Json;
using Refit;

namespace BFF.Web.Domain;

public class RegisterPersonEntryModel
{
    [JsonProperty("id")]
    [AliasAs("id")]
    public Guid? Id { get; set; }

    [JsonProperty("cpf")]
    [AliasAs("cpf")]
    public string Cpf { get; set; }  = string.Empty;

    [JsonProperty("name")]
    [AliasAs("name")]
    public string Name { get; set; }  = string.Empty;

    [JsonProperty("birth_date")]
    [AliasAs("birth_date")]
    public DateTime? BirthDate { get; set; }

    [JsonProperty("email")]
    [AliasAs("email")]
    public string Email { get; set; } = string.Empty;

    [JsonProperty("phone_number")]
    [AliasAs("phone_number")]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [JsonProperty("can_expose_data")]
    [AliasAs("can_expose_data")]
    public bool? CanExposeData { get; set; }
    
    [JsonProperty("can_receive_message")]
    [AliasAs("can_receive_message")]
    public bool? CanReceiveMessage { get; set; }

    [JsonProperty("person_type")]
    [AliasAs("person_type")]
    public PersonType? PersonType { get; set; }
    
    [JsonProperty("employment")]
    [AliasAs("employment")]
    public EmploymentEntryModel Employment { get; set; } = null!;
    
    [JsonProperty("continuing_education")]
    [AliasAs("continuing_education")]
    public ContinuingEducationEntryModel ContinuingEducation { get; set; } = null!;
    
    [JsonProperty("address")]
    [AliasAs("address")]
    public AddressEntryModel Address { get; set; } = null!;
}

public class EmploymentEntryModel
{
    [JsonProperty("role")]
    [AliasAs("role")]
    public string Role { get; set; } = string.Empty;

    [JsonProperty("enterprise")]
    [AliasAs("enterprise")]
    public string Enterprise { get; set; } = string.Empty;

    [JsonProperty("salary_range")]
    [AliasAs("salary_range")]
    public decimal? SalaryRange { get; set; }

    [JsonProperty("is_public_initiative")]
    [AliasAs("is_public_initiative")]
    public bool? IsPublicInitiative { get; set; }
    
    [JsonProperty("is_public")]
    [AliasAs("is_public")]
    public bool? IsPublic { get; set; }

    [JsonProperty("start_date")]
    [AliasAs("start_date")]
    public DateTime? StartDate { get; set; }

    [JsonProperty("end_date")]
    [AliasAs("end_date")]
    public DateTime? EndDate { get; set; }
}

public class ContinuingEducationEntryModel
{
    [JsonProperty("is_public")]
    [AliasAs("is_public")]
    public bool? IsPublic { get; set; }
    
    [JsonProperty("has_certification")]
    [AliasAs("has_certification")]
    public bool? HasCertification { get; set; }
    
    [JsonProperty("has_specialization")]
    [AliasAs("has_specialization")]
    public bool? HasSpecialization { get; set; }
    
    [JsonProperty("has_master_degree")]
    [AliasAs("has_master_degree")]
    public bool? HasMasterDegree { get; set; }
    
    [JsonProperty("has_doctorate_degree")]
    [AliasAs("has_doctorate_degree")]
    public bool? HasDoctorateDegree { get; set; }
}

public class AddressEntryModel
{
    [JsonProperty("state")]
    [AliasAs("state")]
    public string State { get; set; } = string.Empty;

    [JsonProperty("country")]
    [AliasAs("country")]
    public string Country { get; set; } = string.Empty;
    
    [JsonProperty("is_public")]
    [AliasAs("is_public")]
    public bool? IsPublic { get; set; }
}