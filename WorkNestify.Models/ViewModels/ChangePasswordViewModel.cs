using System.ComponentModel.DataAnnotations;

namespace WorkNestify.Models.ViewModels;

public class ChangePasswordViewModel
{
    public string Id { get; set; }

    [Required(ErrorMessage = "New password is required")]
    [DataType(DataType.Password)]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "Password confirmation is required")]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
}