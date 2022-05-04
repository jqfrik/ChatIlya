namespace Chat.Bll.Dtos;

public class AddFriendRequest
{
    public Guid CurrentUserId { get; set; }
    
    public Guid FriendId { get; set; }
}