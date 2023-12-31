using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebSocket1.Models;

namespace WebSocket1.Controllers;

[ApiController]
[Route("[controller]")]
public class WebSocketsController : ControllerBase
{
private new const int BadRequest = ((int)HttpStatusCode.BadRequest);
private readonly ILogger<WebSocketsController> _logger;

public WebSocketsController(ILogger<WebSocketsController> logger)
{
    _logger = logger;
}

[HttpGet("/ws/{id}")]
public async Task Get(int id)
{
    var game = GameGenerationController.GetGame(id);
    if(game is null)
    {
        HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        return;
    }
    
    if (HttpContext.WebSockets.IsWebSocketRequest)
    {
        using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
        _logger.Log(LogLevel.Information, "WebSocket connection established");
        await Echo(webSocket, game, id);
    }
    else
    {
        HttpContext.Response.StatusCode = BadRequest;
    }
}

private string BufferToString(byte[] buffer)
{
    var msg = "";
    for (var i = 0; i < buffer.Length; i++)
    {
        if (buffer[i] == 0) break;
        msg += (char)buffer[i];
    }

    return msg;
}

private async Task Echo(WebSocket webSocket, Game game, int id)
{
    var buffer = new byte[1024 * 4];
    
    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
    _logger.Log(LogLevel.Information, "Message received from Client");

    while (!result.CloseStatus.HasValue)
    {
        // var serverMsg = Encoding.UTF8.GetBytes($"Server: Hello. You said: {BufferToString(buffer)}");
        var serverMsg = Encoding.UTF8.GetBytes(BufferToString(buffer));
        await webSocket.SendAsync(new ArraySegment<byte>(serverMsg, 0, serverMsg.Length), 
            result.MessageType, result.EndOfMessage, CancellationToken.None);
        _logger.Log(LogLevel.Information, "Message sent to Client");
        
        buffer = new byte[1024 * 4];
        result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        
        _logger.Log(LogLevel.Information, "Message received from Client");

        var msg = BufferToString(buffer);

        try
        {
            // JsonSerializer.Deserialize works only with propriety no with fields 
            var p = JsonSerializer.Deserialize<Player>(msg); // to convert json string in to object; webSocket.send(JSON.stringify(obj))
            _logger.Log(LogLevel.Information, $"msg: {msg}, Player {p}");
        }
        catch
        {
            _logger.Log(LogLevel.Error, $"Fail, {msg}");
        }
        
    }
    await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
    _logger.Log(LogLevel.Information, "WebSocket connection closed");
}
}