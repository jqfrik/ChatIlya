namespace Chat.Bll.Requests;

public class RegisterRequest
{
    public string Name { get; set; }
    
    public string Login { get; set; }
    
    public string Password { get; set; }
}