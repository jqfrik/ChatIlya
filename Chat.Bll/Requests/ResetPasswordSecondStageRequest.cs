namespace Chat.Bll.Requests;

public class ResetPasswordSecondStageRequest
{
    public string Email { get; set; }
    
    public string SmsChecker { get; set; }
}