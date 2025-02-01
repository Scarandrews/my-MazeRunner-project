using System;
using  MazerRunner;
using Spectre.Console;

partial class MazeGenerator  //methods of maze generator
{
    // Aquí se definirían las demás funciones mencionadas en el código original
    private void BlockLastRowExceptExit()
    {
        for (int x = 0; x<Width-1; x++)
        {
            maze[Height-1, x] = WALL;
        }
    }

    private void PlaceRandomObstaclesAndTraps(int count, (int y, int x) finalCell)
    {
        // Teleport Trap
        var teleportTrap = new Trap(
            "Teleport Trap",
            "You are teleported to a random location!",
            (player)=>
            {
                int newX = rand.Next(1,Width-1);
                int newY = rand.Next(1,Height-1);
                while(maze[newY, newX]==WALL)
                {
                    newX = rand.Next(1,Width-1);
                    newY = rand.Next(1,Height-1);
                }
                player.X = newX;
                player.Y = newY;
                System.Console.WriteLine($"{player.Name}was teleported to({player.X}, {player.Y})!");
            }
        );

                // Boost Trap
        var boostTrap = new Trap(
            "Boost Trap",
            "You gain a speed boost for 3 turns!",
            (player) =>
            {
                player.SpeedBoost = true;
                player.TurnsBoosted = 3; // Boost lasts for 3 turns
                Console.WriteLine($"{player.Name} gained a speed boost!");
            }
        );

        // Paralyze Trap
        var paralyzeTrap = new Trap(
            "Paralyze Trap",
            "You are paralyzed for 2 turns!",
            (player) =>
            {
                player.Paralyzed = true;
                player.TurnsParalyzed = 2; // Paralysis lasts for 2 turns
                Console.WriteLine($"{player.Name} was paralyzed for 2 turns!");
            }
        );

       
        // Create a list to store the positions of obstacles and traps
        var obstaclePositions = new List<(int y, int x)>();

        // Loop until we have placed the desired number of obstacles and traps
        for (int i = 0; i < count; i++)
        {
            // Generate a random position within the maze
            var randomX = rand.Next(1, Width - 2);
            var randomY = rand.Next(1, Height - 2);

            // Check if the generated position is not the final cell and is not already occupied by an obstacle or trap
            while (obstaclePositions.Contains((randomY, randomX)) || (randomX, randomY) == finalCell)
            {
                randomX = rand.Next(1, Width - 2);
                randomY = rand.Next(1, Height - 2);
            }

            // Add the position to the list of obstacle positions
            obstaclePositions.Add((randomY, randomX));

            // Randomly decide whether to place an obstacle or a trap at the generated position
            if (rand.NextDouble() < 0.5)
            {
                // Place an obstacle (represented by a '#')
                maze[randomY, randomX] = WALL;
            }
            else
            {
                // Place a trap (represented by a 'T')
                maze[randomY, randomX] = 3; // Use a unique value to represent traps

                // Create a new trap object and add it to the list of traps
                var trap = new Trap(
                    "Random Trap",
                    "A random trap that will hinder your progress.",
                    (player)=>
                    { int effect = rand.Next(-5, 5);
                      if(effect < 0)
                      {
                        System.Console.WriteLine($"{player.Name} loses {Math.Abs(effect)} health points!");
                      }
                      else if (effect>0)
                      {
                        System.Console.WriteLine($"{player.Name} gains {effect} health points!");
                      }
                    }
                    );
                trap.X = randomX;
                trap.Y = randomY;
                traps.Add(trap);
            }
        }
    }

    private void MakeEdgesInaccessible()
    {
        //establece las celdas en la primera y ultima fila inaccesibles
        for(int x = 0; x < Width; x++)
        {
            maze[0,x]= WALL;
            maze[Height-1,x]= WALL;
        }

        // establece las celdas en ka primera y ultima columna como paredes
        for(int y = 0; y < Height; y++)
        {
            maze[y,0]=WALL;
            maze[y, Width-1]=WALL;
        }

        // Asegúrate de que los personajes no puedan caminar sobre las paredes
        if (player1.X == 0 || player1.X == Width - 1 || player1.Y == 0 || player1.Y == Height - 1)
        {
            player1.X = 1;
            player1.Y = 1;
        }

        if (player2.X == 0 || player2.X == Width - 1 || player2.Y == 0 || player2.Y == Height - 1)
        {
            player2.X = 1;
            player2.Y = 1;
        }

        // Implementación para hacer inaccesibles los bordes del laberinto
    }

    private void AsegurarMetaDesbloqueada()
    {
        // Implementación para asegurar que la meta no esté bloqueada por trampas de teletransportación
    }

    public void PrintMaze()
    {
    
    for (int y = 0; y < Height; y++)
    {
        for (int x = 0; x < Width; x++)
        {
            if ( player1!= null && x == player1.X && y == player1.Y)
            {
                Console.Write(player1.Character.Token + " ");
            }
            else if (player2 != null && x == player2.X && y == player2.Y)
            {
                Console.Write(player2.Character.Token + " ");
            }
            else if (maze[y, x] == WALL)
            {
                Console.Write("# "); // Print wall as '#'
            }
            else if (maze[y, x] == PATH)
            {
                // Verificar si hay un objetivo en esta posición
                var objective = objectives.FirstOrDefault(o => o.X == x && o.Y == y && !o.Collected);
                if (objective != null)
                {
                    Console.Write("* "); // Objetivo no recolectado
                }
                else
                {
                    Console.Write("  ");
                }
            }
            else if (maze[x,y]==2) //exit
            {
                Console.Write("E ");
            }
            else if( maze[y,x]==3) //trap
            {
                Console.Write("T ");
            }
            else
            {
                Console.Write("? "); // Print unknown cell as '?'
            }
        }
        Console.WriteLine();
    }
    }

    private Player player1;
    private Player player2;

    public void IniatilazePlayer()
    {
        Console.Write("Enter Player1 name: ");
        string name1 = Console.ReadLine()??string.Empty;
        System.Console.WriteLine("Enter Player2 name: ");
        string name2 = Console.ReadLine()??string.Empty;

        System.Console.WriteLine("Avaible Champions:");
        for (int i =0; i< characters.Count; i++ )
        {
            System.Console.WriteLine($"{i+1}. {characters[i].Name}-{characters[i].Token}-{characters[i].Skill}-{characters[i].Description}");
        }
        
        Console.Write("Enter Player 1 champion number: ");
        int characterNumber1 = Convert.ToInt32(Console.ReadLine()) - 1;
        player1 = new Player(name1, characters[characterNumber1].Token, 1, 1);
        player1.Character = characters[characterNumber1];

        Console.Write("Enter Player 2 champion number: ");
        int characterNumber2 = Convert.ToInt32(Console.ReadLine()) - 1;
        player2 = new Player(name2, characters[characterNumber2].Token, Width-2, 1);
        player2.Character = characters[characterNumber2];

        PrintMaze();
    }

    private List<Character> characters = new List<Character>()
    {
        new Character("Okarun", 'O',"Turbo Boost", "can move two straight places"),
        new Character("Momo Ayase", 'M', "Spiritual Protection", "ignore traps effects"),
        new Character("Turbo Granny",'T', "Demon Speed", "Teleports to a cell adjacent to a random objective"),
        new Character("Acro Silky", 'S', "Dancing Queen", "Puede saltar paredes durante 3 turnos"),
        new Character("Jiji", 'J', "Berseker", "can select a trap and make it inactive for two turns")
    };

    private void CheckForTrap(Player player)
    {
        if (player.SpiritualProtection)
        {
            Console.WriteLine($"{player.Name} is protected from traps!");
            return;
        }
        foreach (var trap in traps)
        {
            if (player.X == trap.X && player.Y == trap.Y)
            {
                trap.ApplyEffect(player);
                break; // Exit after applying the first trap's effect
            }
        }
    }
    public List<Objective> objectives = new List<Objective>();
    

        // Inicializa los objetivos en el laberinto
    private void InitializeObjectives()
    {
        // Agrega objetivos en posiciones aleatorias del laberinto
        for (int i = 0; i < 7; i++)
        {
            int x = rand.Next(1, Width - 1);
            int y = rand.Next(1, Height - 1);
            while(maze[y, x] != PATH || (x == Width - 2 && y == Height - 2))
            {
                x = rand.Next(1, Width - 1);
                y = rand.Next(1, Height - 1);
            }
            objectives.Add(new Objective { X = x, Y = y, Collected = false });
        }
    }
    // Verifica si un jugador ha recogido un objetivo
    private void CheckForObjective(Player player)
    {
        foreach (var objective in objectives)
        {
            if (player.X == objective.X && player.Y == objective.Y && !objective.Collected)
            {
                objective.Collected = true;
                Console.WriteLine($"Jugador {player.Name} ha recogido un objetivo!");
            } 
        }
    }

}