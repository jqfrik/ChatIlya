using Microsoft.AspNetCore.SignalR;

namespace Chat.Bll;

public class ChatHub : Hub
{
    public override Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;
        var user = Context.User;
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;
        var user = Context.User;
        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessageClient(string message)
    {
        var user = Context.User;
        var connectionId = Context.ConnectionId;
    }

    public async Task SendMessageServer(string message)
    {
        var connectionId = Context.ConnectionId;
    }
}