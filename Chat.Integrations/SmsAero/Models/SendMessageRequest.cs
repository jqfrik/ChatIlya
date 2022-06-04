namespace Chat.Integrations.SmsAero.Domains;

public class SendMessageRequest
{
    public SendMessageRequest(string email, string authToken, string telephoneNumber, string destinationTelephone, string sign, string message)
    {
        Email = email;
        AuthToken = authToken;
        TelephoneNumber = telephoneNumber;
        DestinationTelephone = destinationTelephone;
        Sign = sign;
        Message = message;
    }

    public string Email { get; set; }
    
    public string AuthToken { get; set; }
    
    public string TelephoneNumber { get; set; }
    
    public string DestinationTelephone { get; set; }
    
    public string Sign { get; set; }
    
    public string Message { get; set; }
}