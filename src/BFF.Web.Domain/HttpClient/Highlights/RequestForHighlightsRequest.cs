using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BFF.Web.Domain.HttpClient.Highlights;

public record RequestForHighlightsRequest
{
    public string PersonId { get; set; }
    
    [BindProperty(Name = "title")]
    public string Title { get; set; }

    [BindProperty(Name = "description")]
    public string Description { get; set; }

    [BindProperty(Name = "link")]
    public string Link { get; set; }
    
    [BindProperty(Name = "advertising_image")]
    public IFormFile? AdvertisingImage { get; set; }

    [BindProperty(Name = "veracity_files")]
    public IList<IFormFile>? VeracityFiles { get; set; }
}