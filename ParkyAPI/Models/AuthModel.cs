using System.ComponentModel.DataAnnotations;

namespace ParkyAPI.Models
{
    public class AuthModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
