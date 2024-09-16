namespace JWT_API_BD.Models.custom
{
    public class AuthorizationResponse
    {
        public string JWT { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public string MSG { get; set; }
    }
}
