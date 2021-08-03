using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockPaperScissors
{
    public class Program
    {
        static void Main(string[] args)
        {
            void GameStart()
            {
                Console.WriteLine("Welcome, in this application you will be playing Rock, Paper, Scissors against the computer.");
                Console.WriteLine("But there is a twist: Lizard and Spock! The game will be decided on a best of 7.");
                Console.WriteLine("The rules are: Scissors cut Paper covers Rock crushes Lizard poisons Spock smashes Scissors decapitates Lizard eats Paper disproves Spock vaporizes Rock crushes Scissors");
                Console.WriteLine("Please choose a player name:");
                string playerName = Console.ReadLine();
                Console.WriteLine("Press any key to start the game");
                Console.ReadLine();
                PlayGame(playerName);
            }

            void PlayGame (string player)
            {
                Console.WriteLine("Play " + player);
                Console.ReadLine();
            }

            GameStart();
        }
    }
}
