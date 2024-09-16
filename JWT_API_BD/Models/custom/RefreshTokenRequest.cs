namespace JWT_API_BD.Models.custom
{
    public class RefreshTokenRequest
    {
        public string ExpiredToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
