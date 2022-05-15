namespace Chat.Bll.Dtos;

public class GetAllUsersBySearchStringRequest
{
    public string SearchString { get; set; }
    
    public Guid CurrentUserId { get; set; }
}