namespace HeroesApi.Shared.Extensions;

/// <summary>
/// Extensões para objetos do tipo <see cref="Enumerable"/>
/// </summary>
public static class EnumerableExtensoes
{
    /// <summary>
    /// Verifica se um <see cref="Enumerable"/> é diferente de <c>null</c> e se possui algum item
    /// </summary>
    /// <typeparam name="T">Tipo do objeto</typeparam>
    /// <param name="obj">Objeto</param>
    /// <returns></returns>
    public static bool HasData<T>(this IEnumerable<T> obj) => obj?.Any() == true;
}