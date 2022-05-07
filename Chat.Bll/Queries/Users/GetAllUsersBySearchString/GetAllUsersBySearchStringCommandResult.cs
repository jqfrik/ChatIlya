using Chat.Bll.Domains;

namespace Chat.Bll.Queries.Users.GetAllUsersBySearchString;

public class GetAllUsersBySearchStringCommandResult
{
    public IEnumerable<User> Users { get; set; } 
}