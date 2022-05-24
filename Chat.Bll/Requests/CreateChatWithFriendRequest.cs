namespace Chat.Bll.Requests;

public class CreateChatWithFriendRequest
{
    public Guid CurrentUserId { get; set; }
    
    public Guid FriendUserId { get; set; }
}