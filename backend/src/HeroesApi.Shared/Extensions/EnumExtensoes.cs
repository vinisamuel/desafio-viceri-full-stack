using System.ComponentModel;

namespace HeroesApi.Shared.Extensions;

/// <summary>
/// Extensões para objetos do tipo <see cref="Enum"/>
/// </summary>
public static class EnumExtensoes
{
    /// <summary>
    /// Obtém o valor definido no atributo 'Description' da propriedade. Se não encontrar retorna o valor da propriedade do <see cref="Enum"/>
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string GetDescription(this Enum obj)
    {
        var field = obj.GetType().GetField(obj.ToString());

        if (field != null)
        {
            var attribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attribute != null && attribute.Any())
                return attribute.First().Description;
        }

        return obj.ToString();
    }
}
