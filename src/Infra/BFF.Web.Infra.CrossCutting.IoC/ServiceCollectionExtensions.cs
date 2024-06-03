using System.Text.Json;
using BFF.Web.Domain;
using BFF.Web.Services;
using BFF.Web.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace BFF.Web.Infra.CrossCutting.IoC;

public static class ServiceCollectionExtensions
{
    #region Constants
    private const bool ASSUME_DEFAULT_VERSION_WHEN_UNSPECIFIED = true;
    private const string API_DEFAULT_VERSION_PROPERTY = "ApiDefaultVersion";
    private const bool REPORT_API_VERSIONS = true;
    private const string HEADER_API_VERSION = "X-Version";
    private const string QUERY_STRING_API_VERSION = "api-version";
    private const string MEDIA_TYPE_API_VERSION = "ver";
    private const string FORMAT_API_VERSION = "'v'VVV";
    private const bool SUBSTITUTE_API_VERSION_IN_URL = true;
    private const string DOT = ".";
    private const string CLIENT_SERVICE_CONFIGURATION = "ClientServiceConfiguration";
    private const string AUTH_API_CONFIGURATION = "auth_api";
    private const string EGRESS_API_CONFIGURATION = "egress_api";
    #endregion

    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterRefitClients(configuration);
        services.RegisterHttpClients(configuration);

        services.AddScoped<IHttpClientEgressApi, HttpClientEgressApi>();
    }

    private static void RegisterRefitClients(this IServiceCollection services, IConfiguration configuration)
    {
        var clientServiceConfigurations = configuration.GetSection(CLIENT_SERVICE_CONFIGURATION).Get<List<ClientServiceConfiguration>>();

        var authApiConfiguration = clientServiceConfigurations!.Single(c => c.ApplicationName!.Equals(AUTH_API_CONFIGURATION));
        services.AddRefitClient<IAuthApi>(new RefitSettings {
            ContentSerializer = new NewtonsoftJsonContentSerializer(new Newtonsoft.Json.JsonSerializerSettings())
        }).ConfigureHttpClient(c => c.BaseAddress = new Uri(authApiConfiguration.Host!));

        var egressApiConfiguration = clientServiceConfigurations!.Single(c => c.ApplicationName!.Equals(EGRESS_API_CONFIGURATION));
        services.AddRefitClient<IEgressApi>(new RefitSettings {
            ContentSerializer = new NewtonsoftJsonContentSerializer(new Newtonsoft.Json.JsonSerializerSettings())
        }).ConfigureHttpClient(c => c.BaseAddress = new Uri(egressApiConfiguration.Host!));
    }
    
    private static void RegisterHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        var clientServiceConfigurations = configuration.GetSection(CLIENT_SERVICE_CONFIGURATION).Get<List<ClientServiceConfiguration>>();
        
        var egressApiConfiguration = clientServiceConfigurations!.Single(c => c.ApplicationName!.Equals(EGRESS_API_CONFIGURATION));
        services.AddHttpClient(EGRESS_API_CONFIGURATION, c => c.BaseAddress = new Uri(egressApiConfiguration.Host!));
    }
    
    /// <summary>
    /// Register versioning services
    /// </summary>
    /// <param name="services">Service Collection</param>
    /// <param name="configuration">Configuration file ~ appsettings</param>
    public static void AddApiVersioningConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultVersion = configuration[API_DEFAULT_VERSION_PROPERTY]!.Split(DOT);
        var majorVersion = int.Parse(defaultVersion.First());
        var minorVersion = int.Parse(defaultVersion.Last());

        services.AddApiVersioning(options => {
            options.AssumeDefaultVersionWhenUnspecified = ASSUME_DEFAULT_VERSION_WHEN_UNSPECIFIED;
            options.DefaultApiVersion = new ApiVersion(majorVersion, minorVersion);
            options.ReportApiVersions = REPORT_API_VERSIONS;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new QueryStringApiVersionReader(QUERY_STRING_API_VERSION),
                new HeaderApiVersionReader(HEADER_API_VERSION),
                new MediaTypeApiVersionReader(MEDIA_TYPE_API_VERSION)
            );
        });
        
        services.AddVersionedApiExplorer(setup => {
            setup.GroupNameFormat = FORMAT_API_VERSION;
            setup.SubstituteApiVersionInUrl = SUBSTITUTE_API_VERSION_IN_URL;
        });
    }
}
