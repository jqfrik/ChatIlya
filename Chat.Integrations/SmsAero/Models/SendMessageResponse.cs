namespace Chat.Integrations.SmsAero.Models;

public class SendMessageResponse
{
    public bool Success { get; set; }
    
    public SendMessageResponseData[] Data { get; set; }
    
    public Object Message { get; set; }
}

public class CommonData
{
    public long Id { get; set; }
    
    public string From { get; set; }
    
    public string Text { get; set; }
    
    public int Status { get; set; }
    
    public string ExtendStatus { get; set; }
    
    public string Channel { get; set; }
    
    public decimal Cost { get; set; }
    
    public long DateCreate { get; set; }
    
    public long DateSend { get; set; }
}

public class SendMessageResponseData : CommonData
{
    public string Number { get; set; }
}