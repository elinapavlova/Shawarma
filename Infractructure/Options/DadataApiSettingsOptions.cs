namespace Infractructure.Options
{
    public class DadataApiSettingsOptions
    {
        public const string DadataApiSettings = "DadataApiSettings";
        public string Token { get; set; }
        public string Secret { get; set; }
        public string BaseAddress { get; set; }
    }
}