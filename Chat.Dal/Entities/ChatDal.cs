namespace Chat.Dal.Entities;

public class ChatDal
{
    public Guid? Id { get; set; }
    
    public string? Title { get; set; }
    
    public string? PhotoUrl { get; set; }
    
    public ICollection<UserDal> Users { get; set; }
    
    public ICollection<MessageDal> Messages { get; set; }
}