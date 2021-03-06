namespace Chat.Dal.Entities;

public class MessageDal
{
    public Guid? Id { get; set; }
    
    public string? Text { get; set; }
    
    public DateTime? CreatedDate { get; set; }
    
    public UserDal User { get; set; }
    //
    // public bool IsFile { get; set; }
    //
    // public string FilePath { get; set; }
}