using System.Net.Http.Headers;

namespace HeroesApi.Shared.Factories;

/// <summary>
/// Fábrica para definições de JSON
/// </summary>
public class StringContentFactory
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static StringContent Json<T>(T value, JsonSerializerSettings? settings = null)
    {
        settings ??= JsonFactory.CamelCaseSettings();

        return Json(JsonConvert.SerializeObject(value, settings));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    public static StringContent Json(string json)
    {
        var stringContent = new StringContent(json);
        stringContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        return stringContent;
    }
}
