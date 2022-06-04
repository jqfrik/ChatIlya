namespace Chat.Integrations.SmsAero.Domains;

public class AuthResponse
{
    public bool Success { get; set; }
    
    public object Data { get; set; }
    
    public string Message { get; set; } 
}