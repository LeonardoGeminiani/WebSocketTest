using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

var timer = new PeriodicTimer(TimeSpan.FromMinutes(5));
Thread childthread = new(async () =>
{
    while (await timer.WaitForNextTickAsync())
    {
        // check game id state
    }
});
childthread.Start();

//app.UseHttpsRedirection();

app.UseWebSockets();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();