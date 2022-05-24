namespace Chat.Bll.Requests;

public class CheckChatWithCurrentFriendRequest
{
    public string CurrentUserId { get; set; }
    
    public string FriendUserId { get; set; }
}