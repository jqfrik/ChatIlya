namespace Chat.Bll.Dtos;

public class RemoveFriendRequest
{
    public Guid CurrentUserId { get; set; }
    
    public Guid FriendId { get; set; }
}