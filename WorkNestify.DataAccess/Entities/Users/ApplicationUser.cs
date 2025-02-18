using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace WorkNestify.DataAccess.Entities.Users;

public class ApplicationUser : IdentityUser
{
    [Required]
    [Column(TypeName = "NVARCHAR(100)")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}