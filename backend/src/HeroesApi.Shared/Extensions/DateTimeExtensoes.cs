namespace HeroesApi.Shared.Extensions;

/// <summary>
/// Extensões para objetos do tipo <see cref="DateTime"/>
/// </summary>
public static class DateTimeExtensoes
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static string ToBrazilDateTime(this DateTime date) => date.ToString("dd/MM/yyyy HH:mm:ss");

    /// <summary>
    /// 
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static string ToBrazilDateTime(this DateTime? date) => date.HasValue ? date.Value.ToBrazilDateTime() : string.Empty;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static string ToBrazilDate(this DateTime date) => date.ToString("dd/MM/yyyy");

    /// <summary>
    /// 
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static string ToBrazilDate(this DateTime? date) => date.HasValue ? date.Value.ToBrazilDate() : string.Empty;
}
