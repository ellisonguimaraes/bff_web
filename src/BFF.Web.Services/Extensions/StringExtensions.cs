using Newtonsoft.Json;

namespace BFF.Web.Services;

public static class StringExtensions 
{
    public static T? DeserializeOrDefault<T>(this string content)
    {
        try 
        {
            return JsonConvert.DeserializeObject<T>(content);
        } 
        catch 
        {
            return default;
        }
    }
}