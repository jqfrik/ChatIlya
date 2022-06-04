namespace Chat.Bll.MailService;

public interface IMailService
{
    Task SendMail(string mailFrom, string login, string password,string? securePassword,string nameFrom, string[] emails, string title, string text, bool isHtml);
}