// See https://aka.ms/new-console-template for more information

using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Chat_Client;
using Microsoft.AspNetCore.SignalR.Client;


var client = new HttpClient();

var data = new UserCredentials("dimalogin", "12345");

var response = await client.PostAsync("http://localhost:5000/api/Users/Authentication",new StringContent(JsonSerializer.Serialize(data),Encoding.UTF8,"application/json"),CancellationToken.None);
var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

var connection = new HubConnectionBuilder().WithUrl("http://localhost:5000/chatHub", options =>
{
    options.AccessTokenProvider = () => Task.FromResult(result.access_token);
}).WithAutomaticReconnect().Build();

await connection.StartAsync();

while (true)
{
    var message = Console.ReadLine();
    await Task.Delay(5000);
    if (message == "разрыв")
    {
        await connection.StopAsync();
        await Task.Delay(1000);
        await connection.StartAsync();
    }
    else
    {
        await connection.SendAsync("SendMessageClient", "wow");
    }
}