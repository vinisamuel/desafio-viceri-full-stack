using System.Text.RegularExpressions;

namespace HeroesApi.Shared.Extensions;

public static class StringExtensoes
{
    public static string ExtractNumbers(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        return Regex.Replace(value, @"\D", string.Empty, RegexOptions.None, TimeSpan.FromMilliseconds(100));
    }
}
