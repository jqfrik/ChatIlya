namespace Chat.Bll.Domains;

public class Chat
{
    public Guid? Id { get; set; }
    
    public string Title { get; set; }
    
    public string PhotoUrl { get; set; }
    
    public ICollection<User>? Users { get; set; }
    
    public ICollection<Message> Messages { get; set; }
}