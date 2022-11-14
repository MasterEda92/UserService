using System.ComponentModel.DataAnnotations;

namespace UserService.DbAccess.Models;

public class UserModel
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string UserName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(320)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;
}