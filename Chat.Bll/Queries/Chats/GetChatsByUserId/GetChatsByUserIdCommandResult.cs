namespace Chat.Bll.Queries.Chats.GetChatsByUserId;

public class GetChatsByUserIdCommandResult
{
    public IEnumerable<Domains.Chat> Chats { get; set; } 
}