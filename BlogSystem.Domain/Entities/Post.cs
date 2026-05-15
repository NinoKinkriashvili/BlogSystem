namespace BlogSystem.Domain.Entities;

public class Post : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid UserId { get; set; }  // FK
    public User User { get; set; } =  null!;
}
