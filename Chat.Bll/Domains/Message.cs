namespace Chat.Bll.Domains;

public class Message
{
    public Guid? Id { get; set; }
    
    public string Text { get; set; }
    
    public bool Deleted { get; set; }
    
    public bool Edited { get; set; }
    
    public User User { get; set; }
}