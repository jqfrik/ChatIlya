// See https://aka.ms/new-console-template for more information

using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Chat_Client;
using Microsoft.AspNetCore.SignalR.Client;


var client = new HttpClient();

var data = new UserCredentials("ilya13072000", "");
var secondData = new UserCredentials("bigsinskill", "");

var response = await client.PostAsync("http://localhost:5000/api/Users/Authentication",new StringContent(JsonSerializer.Serialize(data),Encoding.UTF8,"application/json"),CancellationToken.None);
var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

var secondResponse = await client.PostAsync("http://localhost:5000/api/Users/Authentication",new StringContent(JsonSerializer.Serialize(secondData),Encoding.UTF8,"application/json"),CancellationToken.None);
var secondResult = await secondResponse.Content.ReadFromJsonAsync<AuthResponse>();

var connection = new HubConnectionBuilder().WithUrl("http://localhost:5000/chatHub", options =>
{
    options.AccessTokenProvider = () => Task.FromResult(result.access_token);
}).WithAutomaticReconnect().Build();

connection.On<string>("getMessageClient", (message) =>
{
    Console.WriteLine($"Прилетел месседж для первого коннекшена {message}");
});

connection.On<SendMessageClientResponse>("sendMessageClientStatus", (response) =>
{
    Console.WriteLine($"Прилетело подтверждение для первого коннекшена для обновления UI {response.Success}");
});

await connection.StartAsync();

var secondConnection = new HubConnectionBuilder().WithUrl("http://localhost:5000/chatHub", options =>
{
    options.AccessTokenProvider = () => Task.FromResult(secondResult.access_token);
}).WithAutomaticReconnect().Build();

secondConnection.On<string>("getMessageClient", (message) =>
{
    Console.WriteLine($"Прилетел месседж для вторго коннекшена {message}");
});


secondConnection.On<SendMessageClientResponse>("sendMessageClientStatus", (response) =>
{
    Console.WriteLine($"Прилетело подтверждение для второго коннекшена для обновления UI {response.Success}");
});

await secondConnection.StartAsync();

while (true)
{
    var message = Console.ReadLine();
    await connection.SendAsync("sendMessageClient", message,new Guid(secondResult.id));
}