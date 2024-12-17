using System;
using MazeRunner;
using Spectre.Console;
class Laberinto
{
    private int[,] estructura;
    private static Random rand = new Random(12345); // Usamos una semilla fija para obtener el mismo laberinto siempre
    private int salidaX, salidaY;

    public int Width { get; }
    public int Height { get; }

    public Laberinto(int width, int height, double porcentajeParedes)
    {
        Width = width;
        Height = height;
        estructura = new int[height, width];
        salidaX = height / 2; // Colocamos la salida en la mitad del borde derecho
        salidaY = width - 1;  // Borde derecho
        GenerarLaberinto(porcentajeParedes);
    }

    private void GenerarLaberinto(double porcentajeParedes)
    {
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (i == 0 || j == 0 || i == Height - 1 || j == Width - 1)
                {
                    estructura[i, j] = 1; // Pared en los bordes
                }
                else
                {
                    estructura[i, j] = (rand.NextDouble() < porcentajeParedes) ? 1 : 0; // Paredes aleatorias
                }
            }
        }

        estructura[1, 1] = 0; // Entrada
        estructura[salidaX, salidaY] = 0; // Salida en el borde derecho
        CrearCamino(1, 1, salidaX, salidaY);

        // Añadir algunas trampas
        for (int k = 0; k < (Width * Height) * 0.1; k++)
        {
            int trapX = rand.Next(1, Height - 2);
            int trapY = rand.Next(1, Width - 2);
            if (estructura[trapX, trapY] == 0 && (trapX != salidaX || trapY != salidaY))
            {
                estructura[trapX, trapY] = 2; // Trampa
            }
        }
    }

    private void CrearCamino(int xInicio, int yInicio, int xFin, int yFin)
    {
        int x = xInicio, y = yInicio;
        while (x != xFin || y != yFin)
        {
            estructura[x, y] = 0;
            if (x < xFin) x++;
            else if (y < yFin) y++;
            else if (x > xFin) x--;
            else if (y > yFin) y--;
        }
        estructura[xFin, yFin] = 0;
    }

    public void Mostrar(int xJugador1, int yJugador1, int xJugador2, int yJugador2, int xJugador3, int yJugador3)
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
                else if (i == xJugador3 && j == yJugador3)
                {
                    AnsiConsole.Markup("[medium_purple3]J3[/]"); // Jugador 3 en violeta
                }
                else if (estructura[i, j] == 1)
                {
                    AnsiConsole.Markup("[green]▓▓[/]"); // Pared en verde
                }
                else if (estructura[i, j] == 2)
                {
                    AnsiConsole.Markup("[red]✹ [/]"); // Trampa en rojo
                }
                else if (i == salidaX && j == salidaY)
                {
                    AnsiConsole.Markup("[yellow]⛶[/]"); // Indicador de la salida en el borde derecho
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
