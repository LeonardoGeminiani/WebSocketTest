using Microsoft.AspNetCore.Mvc;

namespace WebSocket1.Controllers;

[ApiController]
[Route("[controller]")]
public class GameGenerationController: ControllerBase
{
    struct GameId
    {
        public DateTime Date;
        public bool Status;

        public GameId()
        {
            Date = DateTime.Now;
            Status = false;
        }
    }
    
    // start and end are included
    private const uint StartGameId = 1;
    private const uint EndGameId = 500;
    //private static bool[] GameIds = new bool[EndGameId +1 -StartGameId];
    private static GameId[] GameIds = new GameId[EndGameId + 1 - StartGameId];
    // true when is occupied, false when is free

    private readonly ILogger<GameGenerationController> _logger;

    public GameGenerationController(ILogger<GameGenerationController> logger)
    {
        _logger = logger;
    }

    [HttpGet("CreateGame")]
    public static string CreateGame()
    {
        for (uint i = 0; i < GameIds.Length; ++i)
        {
            if (GameIds[i].Status == false)
            {
                GameIds[i].Status = true;
                GameIds[i].Date = DateTime.Now;
                return i.ToString();
            }
        }

        return "No More Game ID";
    }

    public static void CloseGameId(uint id)
    {
        GameIds[id].Status = false;
    }
}