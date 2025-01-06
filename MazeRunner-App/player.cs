using System;
using MazeRunner;
using Spectre.Console;

class Jugador
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Fichas { get; private set; }

    public Jugador(int xInicial, int yInicial)
    {
        X = xInicial;
        Y = yInicial;
        Fichas = 5;
    }

    public int MaxPasos()
    {
        return Fichas >= 5 ? 3 : (Fichas >= 3 ? 2 : 1);
    }

    public bool Mover(int dx, int dy, Laberinto laberinto)
    {
        int pasos = MaxPasos();
        for (int i = 0; i < pasos; i++)
        {
            int nuevoX = X + dx;
            int nuevoY = Y + dy;

            if (laberinto.EsPosicionValida(nuevoX, nuevoY))
            {
                X = nuevoX;
                Y = nuevoY;

                if (laberinto.EsTrampa(X, Y))
                {
                    Fichas--;
                    AnsiConsole.MarkupLine("[red]¡Has caído en una trampa y perdido una ficha![/]");
                    break;
                }

                if (laberinto.EsSalida(X, Y))
                    return true;
            }
            else
            {
                AnsiConsole.MarkupLine("[grey]Chocaste con una pared y no avanzaste más.[/]");
                break;
            }
        }
        return false;
    }

    public bool SinFichas()
    {
        return Fichas <= 0;
    }
}