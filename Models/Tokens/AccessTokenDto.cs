namespace Models.Tokens
{
    public class AccessTokenDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public long Expiration { get; set; }
    }
}