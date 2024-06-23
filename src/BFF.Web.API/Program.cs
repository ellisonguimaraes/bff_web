using BFF.Web.Infra.CrossCutting.IoC;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Serilog;

#region CONSTANTS
const string APPLICATION_PROPERTY = "Application";
const string APPLICATION_NAME = "ApplicationName";
const string HEALTH_CHECK_ROUTE = "/health";
const string PAGINATION_HEADER_NAME = "X-Pagination";
const string STATIC_FILE_REQUEST_PATH = "/archives";
const string STATIC_FILE_LOCAL_FILES_PATH = "files";
const string BEARER_AUTHENTICATION = "BearerAuthentication";
#endregion

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Serilog Configuration
builder.Logging.ClearProviders();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithProperty(APPLICATION_PROPERTY, builder.Configuration[APPLICATION_NAME]!)
    .CreateLogger();

builder.Logging.AddSerilog(logger);

// Register services
builder.Services.RegisterServices(builder.Configuration);

// Versioning Configuration
builder.Services.AddApiVersioningConfiguration(builder.Configuration);

// Healt Check Configuration
builder.Services.AddHealthChecks();

// Clear .NET Built-in Validator (in the action inside the controller)
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Authentication Configuration
builder.Services.AddAuthentication(BEARER_AUTHENTICATION)
    .AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>(BEARER_AUTHENTICATION, null);

// Cors Configuration
builder.Services.AddCors();

var app = builder.Build();

app.UseCors(opt => opt
    .AllowAnyMethod()
    .AllowAnyHeader().WithExposedHeaders(PAGINATION_HEADER_NAME)
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

// Configure static files
var completePath = Path.Combine(Path.GetPathRoot(Directory.GetCurrentDirectory())!, STATIC_FILE_LOCAL_FILES_PATH);

app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(completePath),
    RequestPath = STATIC_FILE_REQUEST_PATH,
    EnableDefaultFiles = true
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.MapHealthChecks(HEALTH_CHECK_ROUTE);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
