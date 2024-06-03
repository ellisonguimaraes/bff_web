using BFF.Web.Domain;
using BFF.Web.Domain.HttpClient.Testimony;
using Refit;

namespace BFF.Web.Services;

public interface IEgressApi
{
    #region Constants
    private const string GET_RANDOM_QUANTITY_PARAMETER = "quantity";
    private const string ID_PARAMETER = "id";
    private const string PAGE_NUMBER = "page_number";
    private const string PAGE_SIZE = "page_size";
    private const string QUERY_STRING = "query";
    private const string ORDER_BY = "order_by";
    private const string PERSON_ID = "person_id";
    private const string PERFIL_IMAGE = "perfil_image";
    private const string ID = "id";
    private const string PERSON_ID_HEADER = "Person-Id";
    private const string BATCH = "batch";
    #endregion

    [Get("/api/v1/highlights/random/{quantity}")]
    Task<HttpResponseMessage> GetRandomHighlightsAsync([AliasAs(GET_RANDOM_QUANTITY_PARAMETER)] int quantity);

    [Get("/api/v1/highlights/all")]
    Task<HttpResponseMessage> GetPaginateHighlightsAsync(
        [AliasAs(PAGE_NUMBER)] int pageNumber,
        [AliasAs(PAGE_SIZE)] int pageSize,
        [AliasAs(QUERY_STRING)] string query,
        [AliasAs(ORDER_BY)] string orderByProperty);

    [Put("/api/v1/highlights/accept/{id}")]
    Task<HttpResponseMessage> AcceptHighlightsAsync([AliasAs(ID_PARAMETER)] string id);
    
    [Delete("/api/v1/highlights/{id}")]
    Task<HttpResponseMessage> DeleteHighlightsAsync([AliasAs(ID_PARAMETER)] string id);
    
    [Delete("/api/v1/testimony/{id}")]
    Task<HttpResponseMessage> DeleteTestimonyAsync([AliasAs(ID_PARAMETER)] string id);
    
    [Put("/api/v1/testimony/accept/{id}")]
    Task<HttpResponseMessage> AcceptTestimonyAsync([AliasAs(ID_PARAMETER)] string id);

    [Post("/api/v1/testimony/request")]
    Task<HttpResponseMessage> RequestTestimonyAsync(
        [Header(PERSON_ID_HEADER)] string personId,
        [Body] RequestForTestimonyRequest request);
    
    [Get("/api/v1/testimony/random/{quantity}")]
    Task<HttpResponseMessage> GetRandomTestimoniesAsync([AliasAs(GET_RANDOM_QUANTITY_PARAMETER)] int quantity);

    [Get("/api/v1/testimony/all")]
    Task<HttpResponseMessage> GetPaginateTestimoniesAsync(
        [AliasAs(PAGE_NUMBER)] int pageNumber,
        [AliasAs(PAGE_SIZE)] int pageSize,
        [AliasAs(QUERY_STRING)] string query,
        [AliasAs(ORDER_BY)] string orderByProperty);

    [Get("/api/v1/person/egress-per-year")]
    Task<HttpResponseMessage> GetCountEgressPerFinalSemesterAsync();

    [Get("/api/v1/egress/all")]
    Task<HttpResponseMessage> GetPaginateEgressAsync(
        [AliasAs(PAGE_NUMBER)] int pageNumber,
        [AliasAs(PAGE_SIZE)] int pageSize,
        [AliasAs(QUERY_STRING)] string query,
        [AliasAs(ORDER_BY)] string orderByProperty);

    [Post("/api/v1/person/register")]
    Task<HttpResponseMessage> RegisterPersonAsync([Body] RegisterPersonEntryModel request);
    
    [Put("/api/v1/person")]
    Task<HttpResponseMessage> UpdatePersonAsync([Body] RegisterPersonEntryModel request);
    
    [Multipart]
    [Put("/api/v1/person/profile-image")]
    Task<HttpResponseMessage> UpdateProfileImageAsync([AliasAs(PERSON_ID)] string personId, [AliasAs(PERFIL_IMAGE)] StreamPart image);
    
    [Get("/api/v1/person/{id}")]
    Task<HttpResponseMessage> GetPersonByIdAsync([AliasAs(ID)] string id);
    
    [Delete("/api/v1/person/{id}")]
    Task<HttpResponseMessage> DeletePersonAsync([AliasAs(ID)] string id);

    [Post("/api/v1/person")]
    Task<HttpResponseMessage> CreateBasicPersonAsync([Body] CreateBasicPersonRequestApi request);
    
    [Multipart]
    [Post("/api/v1/person/batch")]
    Task<HttpResponseMessage> CreateBasicPersonBatchAsync([AliasAs(BATCH)] StreamPart file);
    
    [Get("/api/v1/person/all")]
    Task<HttpResponseMessage> GetPaginatePersonAsync(
        [AliasAs(PAGE_NUMBER)] int pageNumber,
        [AliasAs(PAGE_SIZE)] int pageSize,
        [AliasAs(QUERY_STRING)] string query,
        [AliasAs(ORDER_BY)] string orderByProperty);
    
    [Get("/api/v1/course")]
    Task<HttpResponseMessage> GetAllCoursesAsync();
}