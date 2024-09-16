namespace JWT_API_BD.Models.custom
{
    public class AuthorizationRequest
    {
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
}
