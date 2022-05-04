namespace Chat.Dal.Entities;

public class MessageDal
{
    public Guid? Id { get; set; }
    
    public string Text { get; set; }
    
    public bool Deleted { get; set; }
    
    public bool Edited { get; set; }
}