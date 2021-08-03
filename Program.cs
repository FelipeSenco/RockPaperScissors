﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RockPaperScissors.Models;
using RockPaperScissors.Repositories;

namespace RockPaperScissors
{    
    public class Program
    {                
        static void Main(string[] args)
        {
            void GameStart()
            {
                Console.WriteLine("Welcome, in this application you will be playing Rock, Paper, Scissors against the computer.");
                Console.WriteLine("But there is a twist: Lizard and Spock! The game will be decided on a best of 7: first to win 4 rounds is the Winner.");
                Console.WriteLine("The rules are: Scissors cut Paper covers Rock crushes Lizard poisons Spock smashes Scissors decapitates Lizard eats Paper disproves Spock vaporizes Rock crushes Scissors");
                Console.WriteLine("Please choose a player name:");
                string playerName = Console.ReadLine();
                Console.WriteLine("Press any key to start the game");
                Console.ReadLine();

                GameContext newGame = new GameContext(playerName);
                newGame.PlayGame();
            }
            GameStart();
        }
    }
   
    //Game class
    public class GameContext
    {
        IApplicationRepository applicationrepository;
        public string  player;
        public int turnCount;//global variable to track the turns
        //Constructor
        public GameContext(string player)
        {
            this.player = player;
            this.turnCount = 0;//resetting turn count
            applicationrepository = new ApplicationRepository();
        }        
        public void PlayGame()
        {            
            Game game = new Game();//instantiating object from Game model class
            game.PlayerTurnWins = 0;//resetting count
            game.ComputerTurnWins = 0;//resetting count
            game.TurnDraws = 0;//resetting count  
            game.PlayerName = this.player;
            game.GameID = applicationrepository.GetLatestGameID();

            while (game.PlayerTurnWins < 4 || game.ComputerTurnWins < 4)//keep playing turns until one player reach 4 turn wins
            {
                TurnContext newTurnContext = new TurnContext(game.PlayerName, game.GameID);
                Turn newTurn = newTurnContext.PlayTurn();

                if (newTurn.PlayerChoice == "rock" || newTurn.PlayerChoice == "paper" || newTurn.PlayerChoice == "scissors" || newTurn.PlayerChoice == "lizard" || newTurn.PlayerChoice == "spock")
                {                    
                    newTurn.ComputerChoice = GenerateComputerChoice(); ;
                    Console.WriteLine("You chose: " + newTurn.PlayerChoice);
                    Console.WriteLine("Computer chose: " + newTurn.ComputerChoice);
                    this.turnCount++;
                    string result = newTurnContext.TurnResult(newTurn);
                    Console.WriteLine("This was turn number: " + this.turnCount);
                }
                else
                {
                    Console.WriteLine("This option is not valid please choose again.");
                }               
            }
                        
        }

        public void FinishGame(Game game)
        {
            Console.WriteLine("You won " + game.PlayerTurnWins + " turns");
            Console.WriteLine("Computer won " + game.ComputerTurnWins + " turns");
            Console.WriteLine("there were " + game.TurnDraws + " draws");
            if (game.PlayerTurnWins > game.ComputerTurnWins)
            {
                Console.WriteLine("You've won the game!");                
            }
            else
            {
                Console.WriteLine("Computer has won the game!");
            }
        }

        string GenerateComputerChoice()
        {
            var randomFactor = new Random();
            var possibleChoices = new List<string> { "rock", "paper", "scissors", "lizard", "spock" };
            int randomIndex = randomFactor.Next(possibleChoices.Count);
            return possibleChoices[randomIndex];
        }
    }


    //Turn class
    public class TurnContext
    {
        int turnCount;//declaring global variable to store the number of turns
        public string player;
        int GameID;
        IApplicationRepository applicationRepository;
        public TurnContext(string player, int GameID)
        {            
            this.player = player;
            this.GameID = GameID;
            applicationRepository = new ApplicationRepository();
        }
        public Turn PlayTurn()
        {            
            Turn turn = new Turn();//instantianting object from Turn model class            
            turn.PlayerName = this.player;
            turn.PlayerChoice = GetPlayerChoice();
            return turn;            
        }

        public string GetPlayerChoice()
        {
            Console.WriteLine("");
            Console.WriteLine(this.player + " choose an option: Rock, Paper, Scissors, Lizard or Spock (not case sensitive)");
            string playerChoice = Console.ReadLine().ToLower();
            return playerChoice;
        }

        public string TurnResult(Turn turn)
        {
            switch (turn.PlayerChoice)
            {
                case "rock":
                    if (turn.ComputerChoice == "lizard" || turn.ComputerChoice == "scissors")
                    {
                        Console.WriteLine(turn.PlayerChoice + " defeats " + turn.ComputerChoice + ". You won!");                        
                        turn.TurnResult = "Player won";
                    }
                    else if (turn.ComputerChoice == "spock" || turn.ComputerChoice == "paper")
                    {
                        Console.WriteLine(turn.ComputerChoice + " defeats " + turn.PlayerChoice + ". You loose!");
                        turn.TurnResult = "Computer won";
                    }
                    else
                    {
                        Console.WriteLine("it's a draw!");
                        turn.TurnResult = "Draw";
                    }
                    break;

                case "paper":
                    if (turn.ComputerChoice == "rock" || turn.ComputerChoice == "spock")
                    {
                        Console.WriteLine(turn.PlayerChoice + " defeats " + turn.ComputerChoice + ". You won!");
                        turn.TurnResult = "Player won";
                    }
                    else if (turn.ComputerChoice == "scissors" || turn.ComputerChoice == "lizard")
                    {
                        Console.WriteLine(turn.ComputerChoice + " defeats " + turn.PlayerChoice + ". You loose!");
                        turn.TurnResult = "Computer won";
                    }
                    else
                    {
                        Console.WriteLine("it's a draw!");
                        turn.TurnResult = "Draw";
                    }
                    break;

                case "scissors":
                    if (turn.ComputerChoice == "paper" || turn.ComputerChoice == "lizard")
                    {
                        Console.WriteLine(turn.PlayerChoice + " defeats " + turn.ComputerChoice + ". You won!");
                        turn.TurnResult = "Player won";
                    }
                    else if (turn.ComputerChoice == "rock" || turn.ComputerChoice == "spock")
                    {
                        Console.WriteLine(turn.ComputerChoice + " defeats " + turn.PlayerChoice + ". You loose!");
                        turn.TurnResult = "Computer won";
                    }
                    else
                    {
                        Console.WriteLine("it's a draw!");
                        turn.TurnResult = "Draw";
                    }
                    break;

                case "lizard":
                    if (turn.ComputerChoice == "spock" || turn.ComputerChoice == "paper")
                    {
                        Console.WriteLine(turn.PlayerChoice + " defeats " + turn.ComputerChoice + ". You won!");
                        turn.TurnResult = "Player won";
                    }
                    else if (turn.ComputerChoice == "scissors" || turn.ComputerChoice == "rock")
                    {
                        Console.WriteLine(turn.ComputerChoice + " defeats " + turn.PlayerChoice + ". You loose!");
                        turn.TurnResult = "Computer won";
                    }
                    else
                    {
                        Console.WriteLine("it's a draw!");
                        turn.TurnResult = "Draw";
                    }
                    break;

                case "spock":
                    if (turn.ComputerChoice == "scissors" || turn.ComputerChoice == "rock")
                    {
                        Console.WriteLine(turn.PlayerChoice + " defeats " + turn.ComputerChoice + ". You won!");
                        turn.TurnResult = "Player won";
                    }
                    else if (turn.ComputerChoice == "lizard" || turn.ComputerChoice == "paper")
                    {
                        Console.WriteLine(turn.ComputerChoice + " defeats " + turn.PlayerChoice + ". You loose!");
                        turn.TurnResult = "Computer won";
                    }
                    else
                    {
                        Console.WriteLine("it's a draw!");
                        turn.TurnResult = "Draw";
                    }
                    break;
            }
            turn.TurnEndTime = DateTime.Now;
            applicationRepository.InsertTurnInDatabase(turn);

            return turn.TurnResult;
        }        
        
    }
}
