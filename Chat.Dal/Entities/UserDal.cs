namespace Chat.Dal.Entities;

public class UserDal
{
    public Guid? Id { get; set; }
    
    public string? ConnectionId { get; set; }
    
    public string? Login { get; set; }
    public string? Name { get; set; }
    
    public string? Email { get; set; }

    public string? TelephoneNumber { get; set; }

    public string? SmsChecker { get; set; }
    
    public bool Active { get; set; } 
    
    public string? HashPassword { get; set; }
    
    public ICollection<UserDal>? Users { get; set; } = new List<UserDal>();

    public ICollection<ChatDal>? Chats { get; set; } = new List<ChatDal>();
}