using System.Net;
using System.Net.Mail;
using System.Security;
using Microsoft.Extensions.Configuration;

namespace Chat.Bll.MailService;

public class MailService : IMailService
{
    private IConfiguration Configuration { get; }

    public MailService(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public async Task SendMail(string mailFrom,string login, string password,string securePassword,string nameFrom,string[] emails,string title,string text,bool isHtml)
    {
        emails = emails.Where(email => !string.IsNullOrEmpty(email)).ToArray();
        if (emails.Length == 0)
        {
            Console.WriteLine("Нет корректно указанных почт");
            return;
        }
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(mailFrom,nameFrom); // Адрес отправителя
        foreach (var email in emails)
        {
            mail.To.Add(new MailAddress(email)); // Адрес получателя
        }
        
        mail.Subject = title;
        mail.Body = text;
        mail.IsBodyHtml = isHtml;
        SmtpClient client = new SmtpClient();
        client.Host = Configuration["EmailSettings:smtpEmail"];
        client.Port = Convert.ToInt32(Configuration["EmailSettings:smtpEmailPort"]); // Обратите внимание что порт 587
        client.EnableSsl = true;
        var secureString = securePassword;
        var creds = new NetworkCredential(login, password); // Ваши логин и пароль
        var secure = new SecureString();
        for (var i = 0; i < secureString.Length; i++)
        {
            secure.AppendChar(secureString[i]);
        }

        creds.SecurePassword = secure;
        client.Credentials = creds;
        client.Send(mail);
    }
}