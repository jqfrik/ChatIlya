using Chat.Bll.Domains;
using Chat.Dal.Entities;

namespace Chat.Bll.Queries.Users.GetAllUsersBySearchString;

public class GetAllUsersBySearchStringCommandResult
{
    public List<User> Users { get; set; } 
}