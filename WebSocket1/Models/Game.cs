namespace WebSocket1.Models;

public class Game
{
    public DateTime Date { get; private set; }
    public bool Socked { get; private set; }
    public Game()
    {
        Date = DateTime.Now;
    }
}