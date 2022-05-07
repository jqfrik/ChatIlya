using Chat.Bll;
using Chat.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplication();
builder.Services.AddAuthorizationLocal();
builder.Services.AddDbContext<ChatContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")!));
builder.Services.AddSignalR();

var app = builder.Build();

var chatContext = app.Services.GetService<ChatContext>();
await SeedDatabase.Seed(chatContext!);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(options =>
{
    options.MapControllers();
    options.MapHub<ChatHub>("/chatHub");
});

app.Run();