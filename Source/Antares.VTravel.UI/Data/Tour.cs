
public class Tour
{
    public int Id { get; protected set; }
    public int LineId { get; protected set; }
    public string Name { get; protected set; }
    public string? DisplayName { get; protected set; }
    public bool IsEntrance { get; protected set; }
    public bool IsCritical { get; protected set; }
    public bool IsExit { get; protected set; }
    public ApplicationUser User { get; protected set; } = null!;
    public string UserId { get; protected set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public bool Active { get; protected set; }
}