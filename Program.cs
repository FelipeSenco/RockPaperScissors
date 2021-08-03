using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RockPaperScissors.Models;

namespace RockPaperScissors
{
    public class Program
    {
        static void Main(string[] args)
        {
            int playerTurnWins;//global counts to determine the winner
            int computerTurnWins;//global counts to determine the winner
            int turnCount;//global count for total number of turns

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
                playerTurnWins = 0;//resetting count
                computerTurnWins = 0;//resetting count
                turnCount = 0;//resetting count

                while (playerTurnWins < 4 || computerTurnWins < 4)
                {
                    Console.WriteLine("It's your turn " + player + " choose an option: Rock(r), Paper(p), Scissors(s), Lizard(l) or Spock(sp)");
                    string playerChoice = Console.ReadLine().ToLower();
                    PlayTurn(playerChoice);
                    Console.WriteLine(GenerateComputerChoice());
                }                
            }

            void PlayTurn(string playerChoice)
            {
                if (playerChoice == "rock" || playerChoice == "paper" || playerChoice == "scissors" || playerChoice == "lizard" || playerChoice == "spock")
                {
                    Console.WriteLine(playerChoice);
                    turnCount++;
                    Console.WriteLine("This was turn number " + turnCount);
                }
                else
                {
                    Console.WriteLine("bad");        
                }
            }

            string GenerateComputerChoice()
            {
                var randomFactor = new Random();
                var possibleChoices = new List<string> { "rock", "paper", "scissors", "lizard", "spock" };
                int randomIndex = randomFactor.Next(possibleChoices.Count);
                return possibleChoices[randomIndex];
            }            
            GameStart();                        
        }
    }
}
