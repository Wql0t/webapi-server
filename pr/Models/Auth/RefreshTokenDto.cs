using System.ComponentModel.DataAnnotations;

namespace pr.Models.Auth
{
    public class RefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
