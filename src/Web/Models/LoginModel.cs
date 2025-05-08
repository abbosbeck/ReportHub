using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class LoginModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Enter a valid email")]
    public required string Email { get; set; }


    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; set; }
}
