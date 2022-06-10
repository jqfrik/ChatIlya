using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;
using Chat.Integrations.SmsAero.Models;
using Microsoft.Extensions.Configuration;

namespace Chat.Integrations.SmsAero;

public class SmsAeroClient
{
    private readonly HttpClient _client = new();
    private const string DeliveredMessageStatus = "delivery";
    private const int OneSecond = 1000;

    private IConfiguration _configuration { get; }

    public SmsAeroClient(IConfiguration configuration)
    {
        _configuration = configuration;
        var email = _configuration["SmsAeroSettings:email"];
        var authToken = _configuration["SmsAeroSettings:authToken"];
        var authorizationHeaderResult = email + ":" + authToken;
        var bytes = Encoding.UTF8.GetBytes(authorizationHeaderResult);
        var base64String = Convert.ToBase64String(bytes);
        _client.DefaultRequestHeaders.Add("Authorization",string.Format("Basic {0}",base64String));
    }

    public async Task<bool> IsValidAuth()
    {
        var (email, authToken, telephoneNumber, sign) = GetAuthenticationData();

        var response =
            await _client.GetAsync(string.Format("https://{0}:{1}@gate.smsaero.ru/v2/auth", email, authToken));
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        return result != null ? result.Success : false;
    }

    public async Task<bool> SendMessage(string destinationTelephone, string message)
    {
        var (email, authToken, telephoneNumber, sign) = GetAuthenticationData();

        var result = await SendMessageRequest(new SendMessageRequest(email, authToken, telephoneNumber,
            destinationTelephone, sign, message));
        // if (result.Success)
        // {
        //     while (timer.Elapsed < TimeSpan.FromMinutes(6))
        //     {
        //         var checkStatus = await CheckSendMessageStatus(result.Data.FirstOrDefault().Id.ToString());
        //         if (checkStatus.Data.ExtendStatus == DeliveredMessageStatus)
        //         {
        //             result.Data.FirstOrDefault().ExtendStatus = DeliveredMessageStatus;
        //             break;
        //         }
        //         await Task.Delay(OneSecond * 60);
        //     }
        // }

        //return result.Success && result.Data.FirstOrDefault().ExtendStatus == DeliveredMessageStatus;
        return result.Success;
    }

    public async Task<CheckSendMessageStatus> CheckSendMessageStatus(string messageId)
    {
        var (email, authToken, _, _) = GetAuthenticationData();
        email = email.Replace("@", "$40");
        var response = await _client.GetAsync(string.Format("https://{0}:{1}@gate.smsaero.ru/v2/sms/status?id={2}",
            email, authToken, messageId));
        var result = await response.Content.ReadFromJsonAsync<CheckSendMessageStatus>();
        return result;
    }

    private async Task<SendMessageResponse> SendMessageRequest(SendMessageRequest request)
    {
        request.Email = request.Email.Replace("@", "%40");
        var url = string.Format(
            @"https://{0}:{1}@gate.smsaero.ru/v2/sms/send?numbers={2}&numbers[]={3}&sign={4}&text={5}",
            request.Email, request.AuthToken, request.TelephoneNumber, request.DestinationTelephone, request.Sign,
            request.Message);
        var uri = new Uri(url); 
        var response = await _client.GetAsync(url);

        //var result = await response.Content.ReadAsStringAsync();
        var result = await response.Content.ReadFromJsonAsync<SendMessageResponse>();
        return result;
    }

    private (string? email, string? authToken, string? telephoneNumber, string? sign) GetAuthenticationData()
    {
        var email = _configuration["SmsAeroSettings:email"];
        var authToken = _configuration["SmsAeroSettings:authToken"];
        var telephoneNumber = _configuration["SmsAeroSettings:telephoneNumber"];
        var sign = _configuration["SmsAeroSettings:sign"];
        return (email, authToken, telephoneNumber, sign);
    }
}