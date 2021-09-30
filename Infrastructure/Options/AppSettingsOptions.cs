namespace Infrastructure.Options
{
    public class AppSettingsOptions
    {
        public const string AppSettings = "AppSettings";
        public string Secret { get; set; }
        public int DefaultPageSize { get; set; }
    }
}