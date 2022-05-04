using Chat.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Dal;

public sealed class ChatContext : DbContext
{
    public ChatContext(DbContextOptions<ChatContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
    public DbSet<UserDal> Users { get; set; }
    
    public DbSet<ChatDal> Chats { get; set; }
    
    public DbSet<MessageDal> Messages { get; set; }
}