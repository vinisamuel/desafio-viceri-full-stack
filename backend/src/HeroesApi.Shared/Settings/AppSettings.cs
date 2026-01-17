namespace HeroesApi.Shared.Settings;

public class AppSettings
{
    public DatabaseSettings HeroesAppDatabase { get; set; } = null!;

    public (bool IsValid, string ErrorMessage) Validate()
    {
        if (string.IsNullOrWhiteSpace(HeroesAppDatabase?.ConnectionString))
            return (false, "HeroesApp database connection string is not configured.");

        return (true, string.Empty);
    }
}
