namespace Chat.Bll.Domains;

public class Message
{
    public Guid? Id { get; set; }
    
    public string Text { get; set; }
    
    public DateTime? CreatedDate { get; set; }
    
    public User User { get; set; }
}