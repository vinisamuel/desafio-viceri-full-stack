using Newtonsoft.Json.Serialization;

namespace HeroesApi.Shared.Factories;

/// <summary>
/// Fábrica para definições de JSON
/// </summary>
public class JsonFactory
{
    /// <summary>
    /// Configuração para Camel Case
    /// </summary>
    /// <returns></returns>
    public static JsonSerializerSettings CamelCaseSettings()
    {
        return new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }

    /// <summary>
    /// Configuração para Pascal Case
    /// </summary>
    /// <returns></returns>
    public static JsonSerializerSettings PascalCaseSettings()
    {
        return new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
    }
}
