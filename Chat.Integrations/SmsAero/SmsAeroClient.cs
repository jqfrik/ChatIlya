using System.Diagnostics;
using System.Net.Http.Json;
using Chat.Integrations.SmsAero.Domains;
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
        var timer = new Stopwatch();
        timer.Start();
        if (result.Success)
        {
            while (result.Data.ExtendStatus != DeliveredMessageStatus && timer.Elapsed < TimeSpan.FromMinutes(6))
            {
                result = await SendMessageRequest(new SendMessageRequest(email, authToken, telephoneNumber,
                    destinationTelephone, sign, message));
                await Task.Delay(OneSecond * 60);
            }
        }

        timer.Stop();

        return result.Success && result.Data.ExtendStatus == DeliveredMessageStatus;
    }

    public async Task<SendMessageResponse> CheckSendMessageStatus(string messageId)
    {
        var (email, authToken, _, _) = GetAuthenticationData();
        var response = await _client.GetAsync(string.Format("https://{0}:{1}@gate.smsaero.ru/v2/sms/status?id={1}",
            email, authToken, messageId));
        return (await response.Content.ReadFromJsonAsync<SendMessageResponse>())!;
    }

    private async Task<SendMessageResponse> SendMessageRequest(SendMessageRequest request)
    {
        var url = string.Format(
            @"https://{0}/:{1}@gate.smsaero.ru/v2/sms/send?numbers={2}&numbers[]={3}&sign={4}&text={5}",
            request.Email, request.AuthToken, request.TelephoneNumber, request.DestinationTelephone, request.Sign,
            request.Message);
        var uri = new Uri(url); 
        var response = await _client.GetAsync(url);

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