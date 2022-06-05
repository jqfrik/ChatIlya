namespace Chat.Integrations.SmsAero.Models;

public class AuthResponse
{
    public bool Success { get; set; }
    
    public object Data { get; set; }
    
    public string Message { get; set; } 
}