using System;

namespace GuessTheWord
{
    class Program
    {
        //use vowles as metrics to choose words 
        //scrabble values as a metric
        static void Main(string[] args)
        {
            GameSystem GS = new GameSystem();
            GS.StartGame();
        }
    }
}
