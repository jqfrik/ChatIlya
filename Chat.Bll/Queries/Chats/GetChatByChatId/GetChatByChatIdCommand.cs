using Chat.Dal;
using MediatR;

namespace Chat.Bll.Queries.Chats.GetChatByChatId;

public class GetChatByChatIdCommand : IRequest<GetChatByChatIdResult>
{
    public string ChatId { get; }
    public ChatContext Context { get; }

    public GetChatByChatIdCommand(string chatId, ChatContext context)
    {
        ChatId = chatId;
        Context = context;
    }
}