using System.Reflection;
using Chat.Bll.Domains;
using Chat.Dal.Entities;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace Chat.Bll;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection container)
    {
        container.AddMediatR(Assembly.GetExecutingAssembly());
    }

    public static User Convert(this UserDal user)
    {
        return new User()
        {
            Id = user?.Id,
            Active = user.Active,
            Chats = user?.Chats?.Select(x => x?.Convert())?.ToList(),
            Name = user?.Name,
            Users = user?.Users?.Select(x => x?.Convert())?.ToList()
        };
    }

    public static Domains.Chat Convert(this ChatDal chat)
    {
        return new Domains.Chat()
        {
            Id = chat?.Id,
            Messages = chat?.Messages?.Select(x => x?.Convert())?.ToList(),
            Title = chat?.Title,
            Users = chat?.Users?.Select(x => x?.Convert())?.ToList(),
            PhotoUrl = chat?.PhotoUrl
        };
    }
    public static Message Convert(this MessageDal message)
    {
        return new Message()
        {
            Id = message?.Id,
            Deleted = message.Deleted,
            Edited = message.Edited,
            Text = message?.Text
        };
    }

    public static IEnumerable<User> Convert(this IEnumerable<UserDal> users)
    {
        return users?.Select(user => user.Convert());
    }

    public static IEnumerable<Domains.Chat> Convert(this IQueryable<ChatDal> chats)
    {
        return chats?.Select(chat => chat.Convert());
    }
}