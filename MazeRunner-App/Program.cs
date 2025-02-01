using System;
using Microsoft.VisualBasic; 
using Spectre.Console;

namespace MazerRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var mazeGenerator = new MazeGenerator();
            mazeGenerator.GenerateMaze();
            mazeGenerator.PrintMaze();
            mazeGenerator.PlayGame();
            
        }
    }
}