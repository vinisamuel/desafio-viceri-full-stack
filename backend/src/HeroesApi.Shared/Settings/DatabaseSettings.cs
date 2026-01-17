namespace HeroesApi.Shared.Settings;

public class DatabaseSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public int Timeout { get; set; } = 30;
    public bool EnableLog { get; set; }
}
