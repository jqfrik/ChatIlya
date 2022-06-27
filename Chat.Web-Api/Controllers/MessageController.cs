using Microsoft.AspNetCore.Mvc;
using VkNet;
using VkNet.AudioBypassService.Extensions;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace Chat.Web_Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    [HttpPost("AliceVoice")]
    public async Task<IActionResult> AliceVoice()
    {
        // var reader = new StreamReader(Request.Body);
        // var serializedRequest = await reader.ReadToEndAsync();
        // AliceRequest request = JsonConvert.DeserializeObject<AliceRequest>(serializedRequest);
        // var command = request.request.command;
        var services = new ServiceCollection();
        services.AddAudioBypass(); 
        var vkClient = new VkApi(services);
        await vkClient.AuthorizeAsync(new ApiAuthParams()
        {
            Login = "+79265273478",
            Password = "hujikolp01",
            ApplicationId = 8194705,
            Settings = Settings.All
        });
        // TODO: ЧЕРЕЗ ТОКЕН AUTH
        // var vkApiAccessTokenField = vkClient.GetType().GetTypeInfo().DeclaredProperties
        //     .FirstOrDefault(x => x.Name == "AccessToken");
        // var token = "vk1.a.wh3j0s8MLWIpou1FPwvcm8KNTEIJn1Hz7DKOGqlbHjikAylDmVZCX4ofa-sDDOhG_nomP4T57Ht_7HS9DadJxAgeQ7uPs4b_6pv6e5zzL13aT_TpbN2x1YJPjqDRPX5rLhmFaciHVKQAyDu73Fka86a4rmpDeiKqpqyLOcoOSSm9rBnDGX59sfrM1q6LNtqv";
        // vkApiAccessTokenField.SetValue(vkClient,token);

        for (var i = 0; i < 10; i++)
        {
            vkClient.Messages.Send(new MessagesSendParams()
            {
                PeerId = 734809677,
                Message = "Как сам:" + i,
                RandomId = new Random().Next()
            });
            await Task.Delay(1000);
        }
        //var accountInfo = vkClient.Account.GetInfo();
        //vkClient.Account.Unban(408379807);
        //vkClient.Account.
        return Ok();
    }
}