using System.ComponentModel.DataAnnotations;

namespace pr.Models.Auth
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Email { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
