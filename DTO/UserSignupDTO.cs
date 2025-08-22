using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using UsersTeachers.Models;

namespace UsersTeachers.DTO;

public class UserSignupDTO
{
    [NotNull]
    public int Id { get; set; }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "Username should be between 2-50 characters.")]
    public string? Username { get; set; }

    [StringLength(100, ErrorMessage = "Email should not exceed 100 characters.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string? Email { get; set; }

    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters.")]
    [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\d)(?=.*?\W).{8,}$", ErrorMessage = "Password must contain at least one uppercase letter , one lowercase letter, one digit and one special character.")]
    public string? Password { get; set; }

    [StringLength(10, ErrorMessage = "Invalid phonenumber.")]
    public string? PhoneNumber { get; set; }

    [StringLength(50, ErrorMessage = "Institution should not exceed 50 characters.")]
    public string? Institution { get; set; }

    [StringLength(50, ErrorMessage = "Firstname cannot exceed 50 characters.")]
    public string? Firstname { get; set; }

    [StringLength(50, ErrorMessage = "Lastname cannot exceed 50 characters.")]
    public string? Lastname { get; set; }

    public UserRole UserRole { get; set; }
}