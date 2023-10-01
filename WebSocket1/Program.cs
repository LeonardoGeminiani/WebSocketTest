using System.Net;
using WebSocket1.Controllers;
using WebSocket1.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

uint minutes = 5;
var timer = new PeriodicTimer(TimeSpan.FromMinutes(minutes));
Thread childThread = new(async () =>
{
    while (await timer.WaitForNextTickAsync())
    {
        // check game id state
        GameGenerationController.CheckGameIdStatus(minutes);
    }
});

childThread.Start();

//app.UseHttpsRedirection();

app.UseWebSockets();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();