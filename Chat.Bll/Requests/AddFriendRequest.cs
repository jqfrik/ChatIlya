namespace Chat.Bll.Requests;

public class AddFriendRequest
{
    public Guid CurrentUserId { get; set; }
    
    public Guid FriendId { get; set; }
}