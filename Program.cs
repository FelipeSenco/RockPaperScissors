using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RockPaperScissors.Models;
using RockPaperScissors.Services;

namespace RockPaperScissors
{    
    public class Program
    {              
        static void Main(string[] args)
        {            
                Console.WriteLine("Welcome, in this application you will be playing Rock, Paper, Scissors against the computer.");
                Console.WriteLine("But there is a twist: Lizard and Spock! The game will be decided on a best of 7: first to win 4 rounds is the Winner.");
                Console.WriteLine("The rules are: Scissors cut Paper covers Rock crushes Lizard poisons Spock smashes Scissors decapitates Lizard eats Paper disproves Spock vaporizes Rock crushes Scissors");
                Console.WriteLine("");
                Console.WriteLine("Please choose a player name:");
                string playerName = Console.ReadLine();
                Console.WriteLine("Press any key to start the game");
                Console.ReadLine();

                bool gamePlaying = true;
                
                while (gamePlaying) //loop to check if there will be another game after one finishes
                {
                    GameContext newGameContext = new GameContext(playerName);
                    gamePlaying = newGameContext.PlayGame();//this variable will be false if the user decides not play(see PlayAgain() at GameContext.cs)
                }                
           
        }
    }       
}
