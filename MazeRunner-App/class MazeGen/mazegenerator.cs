using System;
using System.Security.Cryptography.X509Certificates;
using MazerRunner;
using Spectre.Console;
    public partial class MazeGenerator
    {
        #region CamposClase y Constructor

    public const int WALL = 1;
    public const int PATH = 0;
    public int Width = 21;
    public int Height = 21;
    public int[,] maze;
    private Random rand = new Random();
    private List<Trap> traps = new List<Trap>();

    // Constructor para inicializar el laberinto
    public MazeGenerator()
    {
        maze = new int[Height, Width];
        player1 = new Player("", ' ', 0, 0);
        player2 = new Player("", ' ', 0, 0);
    }
    #endregion CamposClase

    // Método principal para generar el laberinto
    public void GenerateMaze()
    {
        // Inicializa todas las casillas como paredes
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                maze[y, x] = WALL; // Marca la celda como pared
            }
        }

        //set exit
        maze[Height-2,Width-2]= PATH;
        maze[Height-1, Width-1] =2;

        // Punto de inicio del laberinto
        var startX = 1;
        var startY = 1;
        maze[startY, startX] = PATH; // Marca el punto de inicio como camino

        var frontier = new List<(int x, int y)>();
        AddFrontier(startX, startY, frontier); // Añade las fronteras iniciales

        // Algoritmo de generación de laberinto
        while (frontier.Count > 0)
        {
            Console.Clear();
            var current = frontier[rand.Next(frontier.Count)]; // Selecciona una celda aleatoria de la frontera
            frontier.Remove(current); // Elimina la celda seleccionada de la frontera

            var neighbors = GetNeighbors(current.x, current.y); // Obtiene los vecinos válidos de la celda seleccionada
            if (neighbors.Count > 0)
            { 
                var neighbor = neighbors[rand.Next(neighbors.Count)]; // Selecciona un vecino aleatorio
                maze[current.y, current.x] = PATH; // Marca la celda actual como camino
                maze[(current.y + neighbor.y) / 2, (current.x + neighbor.x) / 2] = PATH; // Marca la celda intermedia como camino
                AddFrontier(current.x, current.y, frontier); // Añade las nuevas fronteras

                // Asegurarse de que hay al menos una vuelta en el camino
                if ((current.x == startX && current.y == startY + 1) || (current.x == startX + 1 && current.y == startY))
                {
                    var next = frontier[rand.Next(frontier.Count)]; // Selecciona una celda adicional de la frontera
                    maze[next.y, next.x] = PATH; // Marca la celda adicional como camino
                    maze[(next.y + neighbor.y) / 2, (next.x + neighbor.x) / 2] = PATH; // Marca la celda intermedia como camino
                    AddFrontier(next.x, next.y, frontier); // Añade las nuevas fronteras
                }
            }
        }

        // Asegurarse de que la casilla final no esté bloqueada por trampas
        maze[Height - 2, Width - 2] = PATH; // Marca la celda final como camino

        BlockLastRowExceptExit(); // Bloquea la última fila excepto la salida
        PlaceRandomObstaclesAndTraps(10, (Height - 2, Width - 2)); // Asegura que las trampas no se coloquen en la casilla final
        MakeEdgesInaccessible(); // Hace inaccesibles los bordes del laberinto
        AsegurarMetaDesbloqueada(); // Hace que nunca una trampa de teletransportación bloquee la salida
        InitializeObjectives();
    }

    // Añade las fronteras (celdas vecinas) de una celda al conjunto de fronteras
    private void AddFrontier(int x, int y, List<(int x, int y)> frontier)
    {
        // Añade la posición a la izquierda si está dentro de los límites y es una pared
        if (x >= 2 && maze[y, x - 2] == WALL)
        {
            frontier.Add((x - 2, y));
        }

        // Añade la posición arriba si está dentro de los límites y es una pared
        if (y >= 2 && maze[y - 2, x] == WALL)
        {
            frontier.Add((x, y - 2));
        }

        // Añade la posición a la derecha si está dentro de los límites y es una pared
        if (x < Width - 2 && maze[y, x + 2] == WALL)
        {
            frontier.Add((x + 2, y));
        }

        // Añade la posición abajo si está dentro de los límites y es una pared
        if (y < Height - 2 && maze[y + 2, x] == WALL)
        {
            frontier.Add((x, y + 2));
        }
    }

    // Obtiene los vecinos válidos de una celda
    private List<(int x, int y)> GetNeighbors(int x, int y)
    {
        var neighbors = new List<(int x, int y)>();
        if (x >= 2 && maze[y, x - 2] == PATH)
        {
            neighbors.Add((x - 2, y));
        }
        if (y >= 2 && maze[y - 2, x] == PATH)
        {
            neighbors.Add((x, y - 2));
        }
        if (x < Width - 2 && maze[y, x + 2] == PATH)
        {
            neighbors.Add((x + 2, y));
        }
        if (y < Height - 2 && maze[y + 2, x] == PATH)
        {
            neighbors.Add((x, y + 2));
        }
        return neighbors;
    }

    

    #region turns

    public void PlayGame()
    {
        IniatilazePlayer();
        

        while (true)
        {
            #region player1
            // Player 1 turn
            // Dentro del turno de cada jugador (después de mover/verificar trampas):

            
            if (player1.X == Width - 1 && player1.Y == Height - 1)
            {
                if (player1.ObjectivesCollected > 3) // Condición: más de 3 objetivos
                {
                    Console.WriteLine($"[VICTORIA] {player1.Name} escapó con {player1.ObjectivesCollected} objetivos. ¡Gana!");
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine($"[DERROTA] {player1.Name} escapó, pero necesitaba al menos 4 objetivos.");
                    Environment.Exit(0);
                }
            }
        
            foreach (var trap in traps)
            {
                if (trap.IsDeactivated)
                {
                    trap.TurnsDeactivated--;
                    if (trap.TurnsDeactivated <= 0)
                    {
                        trap.IsDeactivated = false;
                        Console.WriteLine($"La trampa en ({trap.X}, {trap.Y}) se reactivó.");
                    }
                }
            }

            if (player1.TurnsJumping > 0)
            {
                player1.TurnsJumping--;
                if (player1.TurnsJumping == 0)
                {
                    player1.CanJumpWalls = false;
                    Console.WriteLine($"{player1.Name} ya no puede saltar paredes.");
                }
            }


            if (player1.SpiritualProtection)
            {
                player1.TurnsProtected--;
                if (player1.TurnsProtected == 0)
                {
                    player1.SpiritualProtection = false;
                }
            }
            Console.WriteLine($"Player {player1.Name}'s turn:");
            string direction = Console.ReadLine()??string.Empty;
            if(player1.Paralyzed)
            {
                player1.TurnsParalyzed--;
                if (player1.TurnsParalyzed == 0)
                {
                    player1.Paralyzed = false;
                }
                System.Console.WriteLine($"{player1.Name} is paralyzed and cannot move this turn.");
            }
            else
            {
            Console.WriteLine("Enter direction (W, A, S, D) or use skill (E): ");
            

            if (direction == "E")
            {
                UseSkill(player1);
            }
            else
            {
               // Update player 1 position
               switch (direction)
                {
                    case "W":
                        int newY = player1.Y-1;
                        if (newY >= 0 && (player1.CanJumpWalls||maze[player1.Y - 1, player1.X] != WALL))
                        {
                            player1.Y = newY;
                        }
                        break;
                    case "A":
                        int newX = player1.X-1;
                        if (newX >= 0 && (player1.CanJumpWalls||maze[player1.Y, player1.X - 1] != WALL))
                        {
                            player1.X = newX;
                        }
                        break;
                    case "S":
                     int newYDown = player1.Y + 1;
                        if (newYDown < Height && (player1.CanJumpWalls || maze[player1.Y + 1, player1.X] != WALL))
                        {
                            player1.Y = newYDown;
                        }
                        break;
                    case "D":
                        int newXRight = player1.X +1;
                        if (newXRight < Width && (player1.CanJumpWalls || maze[player1.Y, player1.X + 1] != WALL))
                        {
                            player1.X = newXRight;
                        }
                        break;
                }
                CheckForObjective(player1);
                CheckForTrap(player1); //check if player1 stepped on a trap
            }
            PrintMaze();
        }


            // Check if player 1 has reached the exit
            if (player1.X == Width - 1 && player1.Y == Height - 1)
            {
                Console.WriteLine($"Player {player1.Name} wins!");
                break;
            }
            #endregion player1

            #region PLayer2

            // Player 2 turn
            // Dentro de los bloques de Player 1 y Player 2 (ejemplo para Player 1):
            if (player2.X == Width - 1 && player2.Y == Height - 1)
            {
                if (player2.ObjectivesCollected > 3) // Condición: más de 3 objetivos
                {
                    Console.WriteLine($"[VICTORIA] {player2.Name} escapó con {player2.ObjectivesCollected} objetivos. ¡Gana!");
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine($"[DERROTA] {player2.Name} escapó, pero necesitaba al menos 4 objetivos.");
                    Environment.Exit(0);
                }
            }


            foreach (var trap in traps)
            {
                if (trap.IsDeactivated)
                {
                    trap.TurnsDeactivated--;
                    if (trap.TurnsDeactivated <= 0)
                    {
                        trap.IsDeactivated = false;
                        Console.WriteLine($"La trampa en ({trap.X}, {trap.Y}) se reactivó.");
                    }
                }
            }


            if (player2.TurnsJumping > 0)
            {
                player2.TurnsJumping--;
                if (player2.TurnsJumping == 0)
                {
                    player2.CanJumpWalls = false;
                    Console.WriteLine($"{player2.Name} ya no puede saltar paredes.");
                }
            }

            if (player2.SpiritualProtection)
            {
                player2.TurnsProtected--;
                if (player2.TurnsProtected == 0)
                {
                    player2.SpiritualProtection = false;
                }
            }
            
            Console.WriteLine($"Player {player2.Name}'s turn:");
            if (player2.Paralyzed)
            {
                player2.TurnsParalyzed--;
                if(player2.TurnsParalyzed == 0)
                {
                    player2.Paralyzed = false;
                }
                System.Console.WriteLine($"{player2.Name} is paralyzed and cannot move this turn.");
            }
            else
            {
                Console.WriteLine("Enter direction (W, A, S, D) or use skill (E): ");
            direction = Console.ReadLine()??string.Empty;

            if (direction == "E")
            {
                UseSkill(player2);
            }
            else
            {
               // Update player 2 position
                switch (direction)
                {
                    case "W":
                        int newY = player2.Y-1;
                        if (newY >= 0 && (player2.CanJumpWalls||maze[player2.Y - 1, player2.X] != WALL))
                        {
                            player2.Y = newY;
                        }
                        break;
                    case "A":
                        int newX = player2.X-1;
                        if (newX >= 0 && (player2.CanJumpWalls||maze[player2.Y, player2.X - 1] != WALL))
                        {
                            player2.X = newX;
                        }
                        break;
                    case "S":
                     int newYDown = player2.Y + 1;
                        if (newYDown < Height && (player2.CanJumpWalls || maze[player2.Y + 1, player2.X] != WALL))
                        {
                            player2.Y = newYDown;
                        }
                        break;
                    case "D":
                       int newXRight = player2.X +1;
                        if (newXRight < Width && (player2.CanJumpWalls || maze[player2.Y, player2.X + 1] != WALL))
                        {
                            player2.X = newXRight;
                        }
                        break;
                }
                CheckForObjective(player2);
                CheckForTrap(player2); //check if player 2 stepped on a trap
            }

            }


            

            // Check if player 2 has reached the exit
            if (player2.X == Width - 1 && player2.Y == Height - 1)
            {
                Console.WriteLine($"Player {player2.Name} wins!");
                break;
            }

            #endregion player2

        }
            // En MazeGenerator.cs, dentro de cada movimiento de jugador:
            CheckForObjective(player1); // Después de actualizar su posición
            CheckForObjective(player2);
            PrintMaze();
    }
    #endregion turns

    #region skills
    private void UseSkill(Player player)
    {
        switch (player.Character.Name)
        {
            case "Okarun":
                TurboBoost(player);
                break;
            case "Momo Ayase":
                SpiritualProtection(player);
                break;
            case "Turbo Granny":
               DemonSpeed(player);
                break;
            case "Acro Silky":
                DancingQueen(player);
                break;
            case "Jiji":
                Berseker(player);
            break;
        }
    }

    private void TurboBoost(Player player)
    {
        player.SpeedBoost= true;
        player.TurnsBoosted =3;
        System.Console.WriteLine($"{player.Name} gained a speed boost!");
    }
    private void SpiritualProtection(Player player)
    {
        player.SpiritualProtection = true;
        player.TurnsProtected = 2;
        Console.WriteLine($"{player.Name} gained spiritual protection!");
    }

    private void DemonSpeed(Player player)
    {
            // Filtrar objetivos no recolectados
        var uncollected = objectives.Where(o => !o.Collected).ToList();
        if (uncollected.Count == 0)
        {
            Console.WriteLine("No objectives left to teleport to!");
            return;
        }

        // Seleccionar un objetivo aleatorio
        var target = uncollected[rand.Next(uncollected.Count)];

        // Obtener celdas adyacentes válidas (arriba, abajo, izquierda, derecha)
        var validCells = new List<(int x, int y)>();
        int[][] directions = { new[] {0, 1}, new[] {1, 0}, new[] {0, -1}, new[] {-1, 0} };
        foreach (var dir in directions)
        {
            int newX = target.X + dir[0];
            int newY = target.Y + dir[1];
            if (newX > 0 && newX < Width - 1 && newY > 0 && newY < Height - 1 &&  maze[newY, newX] == PATH)
            {
                validCells.Add((newX, newY));
            }
        }

        // Teletransportar al jugador si hay celdas válidas
        if (validCells.Count > 0)
        {
            var destination = validCells[rand.Next(validCells.Count)];
            player.X = destination.x;
            player.Y = destination.y;
            Console.WriteLine($"{player.Name} teleported to ({player.X}, {player.Y}) near an objective!");
        }
        else
        {
            Console.WriteLine("No valid cells near the objective!");
        }
    }

    private void DancingQueen(Player player)
    {
        player.CanJumpWalls = true;
        player.TurnsJumping = 3; // Dura 3 turnos
        Console.WriteLine($"{player.Name} activó [Dancing Queen] y puede saltar paredes por 3 turnos!");
    }

    private void Berseker(Player player)
    {
            // Mostrar trampas activas
        var activeTraps = traps.Where(t => !t.IsDeactivated).ToList();
        if (activeTraps.Count == 0)
        {
            Console.WriteLine("There are not avaible traps to deactivate");
            return;
        }

        Console.WriteLine("Avaible traps: ");
        for (int i = 0; i < activeTraps.Count; i++)
        {
            Console.WriteLine($"{i + 1}. Trap in: ({activeTraps[i].X}, {activeTraps[i].Y})");
        }

        Console.Write("Select a trap (number): ");
        if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= activeTraps.Count)
        {
            var selectedTrap = activeTraps[choice - 1];
            selectedTrap.IsDeactivated = true;
            selectedTrap.TurnsDeactivated = 2;
            Console.WriteLine($"{player.Name} deactivated the trap in: ({selectedTrap.X}, {selectedTrap.Y}) por 2 turnos.");
        }
        else
        {
            Console.WriteLine("Unavaible selection");
        }
        }
    #endregion Skill


    }