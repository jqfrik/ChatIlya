using System.Text.Json.Serialization;
using Chat.Bll;
using Chat.Dal;
using Chat.Integrations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplication();
builder.Services.AddAuthorizationLocal();
builder.Services.AddDbContext<ChatContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")!));
builder.Services.AddSignalR();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddSmsAero();

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var chatContext = serviceScope.ServiceProvider.GetRequiredService<ChatContext>();
    await SeedDatabase.Seed(chatContext);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder =>
{
    builder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
});
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(options =>
{
    options.MapControllers();
    options.MapHub<ChatHub>("/chatHub");
});

app.Run();