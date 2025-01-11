using System;
using MazeRunner;
using Spectre.Console;
class Laberinto
{
    private int[,] estructura;
    private static Random rand = new Random();
    private int salidaX, salidaY;
    private List <(int , int )> objetivos;
    private List<(int, int)>trampas;


    public int Width { get; }
    public int Height { get; }

    public Laberinto(int width, int height)
    {
        Width = width;
        Height = height;
        estructura = new int[height, width];
        objetivos = new List <(int, int)>();
        trampas = new List <(int,int)>();
        GenerarLaberintoAccesible();
    }

    private void GenerarLaberintoAccesible()
    {
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
               estructura[i,j] =1;
            }
        }

        
        CrearCamino(1,1);

        VerificarAccesibilidadCompleta();

        ColocarObjetivos(3);

        ColocarTrampas(5);

       salidaX = Height-2;
       salidaY = Width-2;
       estructura[salidaX, salidaY] = 0;
       
    }

    private void CrearCamino(int x, int y)
    {
        Stack<(int, int)> stack = new Stack<(int, int)>();
        stack.Push((x,y));
        estructura[x,y]=0;

        int[] dx ={-2,2,0,0};
        int[] dy = {0,0,-2,2};

        while (stack.Count > 0)
        {
            var(cx,cy) = stack.Pop();
            List<int> directions = Enumerable.Range(0,4).OrderBy(_ => rand.Next()).ToList();

            foreach(int dir in directions)
            {
                int nx = cx + dx[dir];
                int ny = cy + dy[dir];

                if (nx>0 && nx<Height && ny>0 && ny<Width && estructura[nx,ny]==1)
                {
                    estructura[(cx + nx) / 2, (cy + ny) / 2] = 0;
                    estructura[nx, ny] = 0;
                    stack.Push((nx, ny));
                }
            }
        }

    }
    private void VerificarAccesibilidadCompleta()
    {
        bool[,]visitado = new bool[Height,Width];
        Queue<(int, int)> queue = new Queue<(int, int)>();
        queue.Enqueue((1,1));
        visitado[1,1] = true;

        while (queue.Count > 0)
        {
            var(x,y) = queue.Dequeue();
            int[] dx = {-1,1,0,0};
            int[] dy = {0,0,-1,1};

            for (int i = 0; i < 4; i++)
            {
                int nx = x + dx[i];
                int ny = y + dy[i];

                if (nx >= 0 && nx < Height && ny >= 0 && ny < Width && !visitado[nx, ny] && estructura[nx, ny] == 0)
                {
                    visitado[nx, ny] = true;
                    queue.Enqueue((nx, ny));
                }

            }
        }

        for (int i =0; i<Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (estructura[i, j] == 0 && !visitado[i, j])
                {
                    // Si hay una casilla libre inaccesible, crear un camino hacia ella
                    CrearCamino(1, 1);
                    VerificarAccesibilidadCompleta();
                    return;
                }
            }
        }

    }

    private void ColocarObjetivos(int cantidad)
    {
        while (objetivos.Count < cantidad)
        {
            int x = rand.Next(1,Height-1);
            int y = rand.Next(1,Width-1);
            if (estructura[x,y] == 0 && !objetivos.Contains((x,y))&&(x !=salidaX || y !=salidaY))
            {
                objetivos.Add((x,y));
            }
        }
    }

     private void ColocarTrampas(int cantidad)
    {
        while (trampas.Count < cantidad)
        {
            int x = rand.Next(1, Height - 1);
            int y = rand.Next(1, Width - 1);
            if (estructura[x, y] == 0 && !objetivos.Contains((x, y)) && !trampas.Contains((x, y)) && (x != salidaX || y != salidaY))
            {
                trampas.Add((x, y));
            }
        }
    }


    public void MostrarLaberinto(int xJugador1, int yJugador1, int xJugador2, int yJugador2)
    {
        Console.Clear();
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (i == xJugador1 && j == yJugador1)
                {
                    AnsiConsole.Markup("[blue]J1[/]"); // Jugador 1 en azul
                }
                else if (i == xJugador2 && j == yJugador2)
                {
                    AnsiConsole.Markup("[orange1]J2[/]"); // Jugador 2 en naranja
                }
                else if (estructura[i, j] == 1)
                {
                    AnsiConsole.Markup(":alien:"); // Pared en verde
                }
                else if (objetivos.Contains((i,j)))
                {
                    AnsiConsole.Markup(":softball:");
                }
                else if (trampas.Contains((i,j)))
                {
                    AnsiConsole.Markup(":bomb:"); // Trampa en rojo
                }
                else if (i == salidaX && j == salidaY)
                {
                    AnsiConsole.Markup(":nazar_amulet:"); // Indicador de la salida 
                }
                else
                {
                    AnsiConsole.Markup("  "); // Espacio vacío
                }
            }
            Console.WriteLine();
        }
    }

    public bool EsPosicionValida(int x, int y)
    {
        return x >= 0 && x < Height && y >= 0 && y < Width && estructura[x, y] != 1;
    }

    public bool EsSalida(int x, int y)
    {
        return x == salidaX && y == salidaY;
    }

    public bool EsTrampa(int x, int y)
    {
        return estructura[x, y] == 2;
    }
}