namespace Chat.Integrations.SmsAero.Domains;

public class SendMessageResponse
{
    public bool Success { get; set; }
    
    public Data Data { get; set; }
    
    public Object Message { get; set; }
}

public class Data
{
    public long Id { get; set; }
    
    public string From { get; set; }
    
    public string Number { get; set; }
    
    public string Text { get; set; }
    
    public int Status { get; set; }
    
    public string ExtendStatus { get; set; }
    
    public string Channel { get; set; }
    
    public decimal Cost { get; set; }
    
    public long DateCreate { get; set; }
    
    public long DateSend { get; set; }
}