using System;
using Spectre.Console;


namespace MazeRunner
{
class Program
{
    static void Main()
    {
        int width = 20;
        int height = 15;
        double porcentajeParedes = 0.3;

        Laberinto laberinto = new Laberinto(width, height, porcentajeParedes);

        Jugador[] jugadores = {
            new Jugador(1, 1),
            new Jugador(1, 1),
            new Jugador(1, 1)
        };

        int jugadorActual = 0;

        while (true)
        {
            laberinto.Mostrar(jugadores[0].X, jugadores[0].Y, jugadores[1].X, jugadores[1].Y, jugadores[2].X, jugadores[2].Y);

            if (laberinto.EsSalida(jugadores[jugadorActual].X, jugadores[jugadorActual].Y))
            {
                AnsiConsole.MarkupLine($"[green]¡Felicidades, Jugador {jugadorActual + 1}! Has encontrado la salida.[/]");
                break;
            }

            if (jugadores[jugadorActual].SinFichas())
            {
                AnsiConsole.MarkupLine($"[red]Jugador {jugadorActual + 1} se ha quedado sin fichas y ha perdido.[/]");
                break;
            }

            AnsiConsole.MarkupLine($"Jugador {jugadorActual + 1} - Fichas: {jugadores[jugadorActual].Fichas} - Pasos máximos permitidos: {jugadores[jugadorActual].MaxPasos()}");
            AnsiConsole.MarkupLine("Movimiento (arriba, abajo, izquierda, derecha):");

            ConsoleKeyInfo tecla = Console.ReadKey(true);
            bool movimientoValido = false;
            switch (tecla.Key)
            {
                case ConsoleKey.UpArrow:
                    movimientoValido = jugadores[jugadorActual].Mover(-1, 0, laberinto);
                    break;
                case ConsoleKey.DownArrow:
                    movimientoValido = jugadores[jugadorActual].Mover(1, 0, laberinto);
                    break;
                case ConsoleKey.LeftArrow:
                    movimientoValido = jugadores[jugadorActual].Mover(0, -1, laberinto);
                    break;
                case ConsoleKey.RightArrow:
                    movimientoValido = jugadores[jugadorActual].Mover(0, 1, laberinto);
                    break;
            }

            if (movimientoValido)
            {
                jugadorActual = (jugadorActual + 1) % jugadores.Length; // Turno al siguiente jugador
            }
        }

        AnsiConsole.MarkupLine("[yellow]Juego terminado.[/]");
    }
}
}



