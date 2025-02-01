using MazerRunner;
using Spectre.Console;

public class Player
{
    public string Name { get; set; }
    public char Token{ get; set; }
    public int X{ get; set; }
    public int Y { get; set; }
    public int ObjectivesCollected{ get; set; }

    

     public Player(string name, char token, int x, int y)
     {
        Name = name;
        Token = token;
        X = x;
        Y = y;
        Character = new Character("", ' ', "", "");

        

     }
    public bool SpeedBoost { get; set; }
    public bool Paralyzed { get; set; }
    public int TurnsParalyzed { get; set; }
    public bool SpiritualProtection { get; set; }
    public int TurnsProtected { get; set; }
    public bool CanJumpWalls { get; set; }
    public int TurnsJumping { get; set; }

    public int TurnsBoosted { get; set; }
    public Character Character { get; set; }
}