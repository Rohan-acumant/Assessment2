using System.ComponentModel.DataAnnotations;

namespace Librarylevel1.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public List<int> BorrowedBookIds { get; set; } = new();
}