using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

//app.UseHttpsRedirection();

app.UseWebSockets();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();