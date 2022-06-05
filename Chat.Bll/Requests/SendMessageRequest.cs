namespace Chat.Bll.Requests;

public class SendMessageRequest
{
    public string FriendId { get; set; }
    
    public string Message { get; set; }
}