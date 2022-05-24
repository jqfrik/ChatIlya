namespace Chat.Bll.Requests;

public class GetAllUsersBySearchStringRequest
{
    public string SearchString { get; set; }
    
    public string CurrentUserId { get; set; }
}