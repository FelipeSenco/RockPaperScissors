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
        public string playerChoice;
        public string computerChoice;
        public string turnResult;
        public DateTime turnEndTime;       
       
        //CONSTRUCTOR
        public TurnContext(string player)
        {
            this.player = player;            
        }
        
        //GetplayerChoice Method
        public void GetPlayerChoice()
        {
            Console.WriteLine("");
            Console.WriteLine(this.player + " choose an option: Rock, Paper, Scissors, Lizard or Spock (not case sensitive)");
            string playerChoice = Console.ReadLine().ToLower();
            this.playerChoice = playerChoice;
        }

        //TurnResult method
        public void GetTurnResult()
        {
            switch (this.playerChoice)
            {
                case "rock":
                    //player wins
                    if (this.computerChoice == "lizard" || this.computerChoice == "scissors")
                    {
                        Console.WriteLine(this.playerChoice + " defeats " + this.computerChoice + ". You won!");
                        this.turnResult = "Player won";
                    }
                    //computer wins
                    else if (this.computerChoice == "spock" || this.computerChoice == "paper")
                    {
                        Console.WriteLine(this.computerChoice + " defeats " + this.playerChoice + ". You loose!");
                        this.turnResult = "Computer won";
                    }
                    //draw
                    else
                    {
                        Console.WriteLine("it's a draw!");
                        this.turnResult = "Draw";
                    }
                    break;

                case "paper":
                    //player wins
                    if (this.computerChoice == "rock" || this.computerChoice == "spock")
                    {
                        Console.WriteLine(this.playerChoice + " defeats " + this.computerChoice + ". You won!");
                        this.turnResult = "Player won";
                    }
                    //computer wins
                    else if (this.computerChoice == "scissors" || this.computerChoice == "lizard")
                    {
                        Console.WriteLine(this.computerChoice + " defeats " + this.playerChoice + ". You loose!");
                        this.turnResult = "Computer won";
                    }
                    //draw
                    else
                    {
                        Console.WriteLine("it's a draw!");
                        this.turnResult = "Draw";
                    }
                    break;

                case "scissors":
                    //player wins
                    if (this.computerChoice == "paper" || this.computerChoice == "lizard")
                    {
                        Console.WriteLine(this.playerChoice + " defeats " + this.computerChoice + ". You won!");
                        this.turnResult = "Player won";
                    }
                    //computer wins
                    else if (this.computerChoice == "rock" || this.computerChoice == "spock")
                    {
                        Console.WriteLine(this.computerChoice + " defeats " + this.playerChoice + ". You loose!");
                        this.turnResult = "Computer won";
                    }
                    //draw
                    else
                    {
                        Console.WriteLine("it's a draw!");
                        this.turnResult = "Draw";
                    }
                    break;

                case "lizard":
                    //player wins
                    if (this.computerChoice == "spock" || this.computerChoice == "paper")
                    {
                        Console.WriteLine(this.playerChoice + " defeats " + this.computerChoice + ". You won!");
                        this.turnResult = "Player won";
                    }
                    //computer wins
                    else if (this.computerChoice == "scissors" || this.computerChoice == "rock")
                    {
                        Console.WriteLine(this.computerChoice + " defeats " + this.playerChoice + ". You loose!");
                        this.turnResult = "Computer won";
                    }
                    //draw
                    else
                    {
                        Console.WriteLine("it's a draw!");
                        this.turnResult = "Draw";
                    }
                    break;

                case "spock":
                    //player wins
                    if (this.computerChoice == "scissors" || this.computerChoice == "rock")
                    {
                        Console.WriteLine(this.playerChoice + " defeats " + this.computerChoice + ". You won!");
                        this.turnResult = "Player won";
                    }
                    //computer wins
                    else if (this.computerChoice == "lizard" || this.computerChoice == "paper")
                    {
                        Console.WriteLine(this.computerChoice + " defeats " + this.playerChoice + ". You loose!");
                        this.turnResult = "Computer won";
                    }
                    //draw
                    else
                    {
                        Console.WriteLine("it's a draw!");
                        this.turnResult = "Draw";
                    }
                    break;
            }                        
        }

    }
}
