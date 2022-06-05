using Chat.Dal.Entities;

namespace Chat.Dal;

public static class SeedDatabase
{
    public static async Task Seed(ChatContext context)
    {
        if (context != null && !context.Users.Any())
        {
            await context.Users.AddRangeAsync(new List<UserDal>()
            {
                new()
                {
                    Name = "Slava",
                    Login = "bigsinskill",
                    Active = false,
                    HashPassword = "",
                    ConnectionId = "",
                    TelephoneNumber = "+79514295341",
                    Email = "pslava2000@mail.ru"
                },
                new()
                {
                    Name = "Ilya",
                    Login = "ilya13072000",
                    Active = false,
                    HashPassword = "",
                    ConnectionId = "",
                    TelephoneNumber = "+79040758776",
                    Email = "ilyachernuha92@gmail.com"
                },
            });
            await context.SaveChangesAsync();
        }

    }
}