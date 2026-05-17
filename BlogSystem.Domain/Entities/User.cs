using BlogSystem.Domain.Enums;

namespace BlogSystem.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; }
    public UserRole Role { get; set; }

    public ICollection<Post> Posts { get; set; } = new List<Post>();
}
