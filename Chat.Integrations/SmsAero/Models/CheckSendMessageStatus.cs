namespace Chat.Integrations.SmsAero.Models;

public class CheckSendMessageStatus
{
    public bool Success { get; set; }
    
    public CheckSendMessageStatusData Data { get; set; }
    
    public Object Message { get; set; }
}

public class CheckSendMessageStatusData : CommonData
{
    public long Number { get; set; }
}