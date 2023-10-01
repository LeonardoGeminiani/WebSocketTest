using System.Net.WebSockets;

namespace WebSocket1.Models;

public enum BriscolaMode
{
    P2=2,
    P3=3,
    P4=4
}

public enum Difficulty
{
    Easy,
    Hard,
    Extreme
}

public class Game
{
    public DateTime Date { get; private set; }
    public bool Socked { get; private set; }
    
    WebSocket[] 
    
    public Game(int playerNumber, BriscolaMode mode)
    {
        Date = DateTime.Now;
    }
}