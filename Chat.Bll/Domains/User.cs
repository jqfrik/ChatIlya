namespace Chat.Bll.Domains;

public class User
{
    public Guid? Id { get; set; }
    
    public string Name { get; set; }
    
    public bool Active { get; set; }

    public ICollection<User> Users { get; set; }
    
    public ICollection<Chat> Chats { get; set; }
}