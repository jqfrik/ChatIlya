using System.Reflection;
using Chat.Bll.Domains;
using Chat.Bll.MailService;
using Chat.Dal.Entities;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Bll;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection container)
    {
        container.AddMediatR(Assembly.GetExecutingAssembly());
        container.AddScoped<IMailService, MailService.MailService>();
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
        var users = new List<User>();
        foreach (var user in chat.Users)
        {
            var currentUser = new User()
            {
                Active = user.Active,
                Email = user.Email,
                Id = user.Id,
                Name = user.Name
            };
            users.Add(currentUser);
        }

        return new Domains.Chat()
        {
            Id = chat?.Id,
            Messages = chat?.Messages?.Select(x => x?.Convert())?.ToList(),
            Title = chat?.Title,
            Users = users,
            PhotoUrl = chat?.PhotoUrl
        };
    }
    public static Message Convert(this MessageDal message)
    {
        var currentUser = new User()
        {
            Active = message.User != null ? message.User.Active : false,
            Email = message?.User?.Email,
            Id = message?.User?.Id,
            Name = message?.User?.Name
        };
        
        return new Message()
        {
            Id = message?.Id,
            Deleted = message.Deleted,
            Edited = message.Edited,
            Text = message?.Text,
            User = currentUser
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
    public static IEnumerable<Domains.Chat> Convert(this IEnumerable<ChatDal> chats)
    {
        return chats?.Select(chat => chat.Convert());
    }

    public static IEnumerable<Message> Convert(this IEnumerable<MessageDal> messages)
    {
        return messages.Select(message => message.Convert());
    }
}