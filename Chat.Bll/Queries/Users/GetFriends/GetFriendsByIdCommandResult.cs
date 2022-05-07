using Chat.Bll.Domains;

namespace Chat.Bll.Queries.Users.GetFriends;

public class GetFriendsByIdCommandResult
{
    public IEnumerable<User> Users { get; set; } 
}