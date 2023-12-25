using System.ComponentModel.DataAnnotations;

namespace TaskWebApiLab.Auth
{
    public class ApiLoginRequest
    {
        public const string Route = "/Login";

        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
