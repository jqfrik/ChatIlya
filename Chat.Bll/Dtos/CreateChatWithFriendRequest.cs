namespace Chat.Bll.Dtos;

public class CreateChatWithFriendRequest
{
    public Guid MainUser { get; set; }
    
    public Guid SecondaryUser { get; set; }
}