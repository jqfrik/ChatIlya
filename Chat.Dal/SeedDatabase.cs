using Chat.Dal.Entities;

namespace Chat.Dal;

public static class SeedDatabase
{
    public static async Task Seed(ChatContext context)
    {
        if (!context.Users.Any() && context != null)
        {
            await context.Users.AddRangeAsync(new List<UserDal>()
            {
                new()
                {
                    Name = "Slava",
                    Login = "bigsinskill",
                    Active = false,
                    HashPassword = "",
                },
                new()
                {
                    Name = "Ilya",
                    Login = "ilya13072000",
                    Active = false,
                    HashPassword = "",

                },
            });
            await context.SaveChangesAsync();
        }

    }
}