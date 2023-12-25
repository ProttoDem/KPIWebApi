using System.ComponentModel.DataAnnotations;

namespace TaskWebApiLab.Auth
{
    public class ApiRegisterRequest
    {
        public const string Route = "/Register";

        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
