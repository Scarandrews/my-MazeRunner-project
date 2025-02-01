using System;
using Spectre.Console;
using MazerRunner;

public class Trap
{
  public string Name { get; set; }
  public string Description { get; set; }
  public bool IsDeactivated { get; set; } 
  public int TurnsDeactivated { get; set; } 

  public int X { get; set; }
  public int Y { get; set; }
  public Action<Player>Effect { get; set; }

  public Trap(string name, string description, Action<Player> effect)
  {
    Name = name;
    Description = description;
    Effect = effect;

  }
  // Method to apply the trap's effect to the player
  public void ApplyEffect(Player player)
  {
    Console.WriteLine($"{player.Name} stepped on a trap: {Name}!");
    Console.WriteLine($"Effect: {Description}");
    Effect(player);
  }
}
