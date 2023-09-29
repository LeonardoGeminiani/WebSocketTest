namespace WebSocket1.Models;

public class Player
{
    public string Name;
    public int Age;

    public override string ToString()
    {
        return $"Name: {Name}, Age:{Age}";
    }
}