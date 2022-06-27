using Microsoft.Extensions.Primitives;

namespace Chat.Bll.Requests;

public class SendAttachmentsRequest
{
    public File[] Files;
}

public class File
{
    public StringValues FileContent { get; set; }
    
    public string FileName { get; set; }
    
    public string FileType { get; set; }
    
    public string FriendId { get; set; }
}