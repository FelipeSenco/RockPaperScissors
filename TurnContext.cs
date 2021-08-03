using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RockPaperScissors.Models;
using RockPaperScissors.Repositories;

namespace RockPaperScissors
{
    public class TurnContext
    {
        //Declaring variables
        public string player;
        IApplicationRepository applicationRepository;

        //CONSTRUCTOR
        public TurnContext(string player)
        {
            this.player = player;
            applicationRepository = new ApplicationRepository();
        }

        //PlayTurn method
        public Turn PlayTurn()
        {
            Turn turn = new Turn();//instantianting object from Turn model class            
            turn.PlayerName = this.player;
            turn.PlayerChoice = GetPlayerChoice();
            return turn;
        }

        //GetplayerChoice Method
        public string GetPlayerChoice()
        {
            Console.WriteLine("");
            Console.WriteLine(this.player + " choose an option: Rock, Paper, Scissors, Lizard or Spock (not case sensitive)");
            string playerChoice = Console.ReadLine().ToLower();
            return playerChoice;
        }

        //TurnResult method
        public Turn TurnResult(Turn turn)
        {
            switch (turn.PlayerChoice)
            {
                case "rock":
                    //player wins
                    if (turn.ComputerChoice == "lizard" || turn.ComputerChoice == "scissors")
                    {
                        Console.WriteLine(turn.PlayerChoice + " defeats " + turn.ComputerChoice + ". You won!");
                        turn.TurnResult = "Player won";
                    }
                    //computer wins
                    else if (turn.ComputerChoice == "spock" || turn.ComputerChoice == "paper")
                    {
                        Console.WriteLine(turn.ComputerChoice + " defeats " + turn.PlayerChoice + ". You loose!");
                        turn.TurnResult = "Computer won";
                    }
                    //draw
                    else
                    {
                        Console.WriteLine("it's a draw!");
                        turn.TurnResult = "Draw";
                    }
                    break;

                case "paper":
                    //player wins
                    if (turn.ComputerChoice == "rock" || turn.ComputerChoice == "spock")
                    {
                        Console.WriteLine(turn.PlayerChoice + " defeats " + turn.ComputerChoice + ". You won!");
                        turn.TurnResult = "Player won";
                    }
                    //computer wins
                    else if (turn.ComputerChoice == "scissors" || turn.ComputerChoice == "lizard")
                    {
                        Console.WriteLine(turn.ComputerChoice + " defeats " + turn.PlayerChoice + ". You loose!");
                        turn.TurnResult = "Computer won";
                    }
                    //draw
                    else
                    {
                        Console.WriteLine("it's a draw!");
                        turn.TurnResult = "Draw";
                    }
                    break;

                case "scissors":
                    //player wins
                    if (turn.ComputerChoice == "paper" || turn.ComputerChoice == "lizard")
                    {
                        Console.WriteLine(turn.PlayerChoice + " defeats " + turn.ComputerChoice + ". You won!");
                        turn.TurnResult = "Player won";
                    }
                    //computer wins
                    else if (turn.ComputerChoice == "rock" || turn.ComputerChoice == "spock")
                    {
                        Console.WriteLine(turn.ComputerChoice + " defeats " + turn.PlayerChoice + ". You loose!");
                        turn.TurnResult = "Computer won";
                    }
                    //draw
                    else
                    {
                        Console.WriteLine("it's a draw!");
                        turn.TurnResult = "Draw";
                    }
                    break;

                case "lizard":
                    //player wins
                    if (turn.ComputerChoice == "spock" || turn.ComputerChoice == "paper")
                    {
                        Console.WriteLine(turn.PlayerChoice + " defeats " + turn.ComputerChoice + ". You won!");
                        turn.TurnResult = "Player won";
                    }
                    //computer wins
                    else if (turn.ComputerChoice == "scissors" || turn.ComputerChoice == "rock")
                    {
                        Console.WriteLine(turn.ComputerChoice + " defeats " + turn.PlayerChoice + ". You loose!");
                        turn.TurnResult = "Computer won";
                    }
                    //draw
                    else
                    {
                        Console.WriteLine("it's a draw!");
                        turn.TurnResult = "Draw";
                    }
                    break;

                case "spock":
                    //player wins
                    if (turn.ComputerChoice == "scissors" || turn.ComputerChoice == "rock")
                    {
                        Console.WriteLine(turn.PlayerChoice + " defeats " + turn.ComputerChoice + ". You won!");
                        turn.TurnResult = "Player won";
                    }
                    //computer wins
                    else if (turn.ComputerChoice == "lizard" || turn.ComputerChoice == "paper")
                    {
                        Console.WriteLine(turn.ComputerChoice + " defeats " + turn.PlayerChoice + ". You loose!");
                        turn.TurnResult = "Computer won";
                    }
                    //draw
                    else
                    {
                        Console.WriteLine("it's a draw!");
                        turn.TurnResult = "Draw";
                    }
                    break;
            }
            turn.TurnEndTime = DateTime.Now;
            return turn;
        }

    }
}
