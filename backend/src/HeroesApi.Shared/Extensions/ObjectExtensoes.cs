namespace HeroesApi.Shared.Extensions;

/// <summary>
/// Extensões para a classe <see cref="object"/>
/// </summary>
public static class ObjectExtensoes
{
    /// <summary>
    /// Converte um objeto em um JSON no formato <see cref="string"/>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="pascalCase"></param>
    /// <returns></returns>
    public static string ToJson(this object obj, bool pascalCase = false)
    {
        if (obj is null)
            return string.Empty;

        return JsonConvert.SerializeObject(obj, pascalCase ? JsonFactory.PascalCaseSettings() : JsonFactory.CamelCaseSettings());
    }

    /// <summary>
    /// Converte uma <see cref="string"/> com um conteúdo JSON para o objeto <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">Tipo do objeto</typeparam>
    /// <param name="json"></param>
    /// <param name="pascalCase"></param>
    /// <returns></returns>
    public static T? FromJson<T>(this string json, bool pascalCase = false)
    {
        if (string.IsNullOrWhiteSpace(json))
            return default;

        return JsonConvert.DeserializeObject<T>(json, pascalCase ? JsonFactory.PascalCaseSettings() : JsonFactory.CamelCaseSettings());
    }
}
