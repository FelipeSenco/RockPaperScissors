using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RockPaperScissors.Models;
using RockPaperScissors.Repositories;

namespace RockPaperScissors
{    
    public enum Choice//Enumerator to use the choices in a strongly typed way instead of using strings that are prone to typing errors
    {
        Rock,
        Paper,
        Scissors,
        Lizard,
        Spock
    }

    public enum Result//enumerator to acces the winner in a strongly typed manner, avoiding typing errors
    {
        Player,// for player wins
        Computer,//for computer wins
        Draw//for draws
    }

    public class TurnContext
    {        
        //Declaring variables
        public string player;
        public Choice playerChoice;//type Choice from the enumerator 
        public Choice computerChoice;//type Choice from the enumerator 
        public Result turnResult;//type Result from the enumerator
        public DateTime turnEndTime;//time when the turn ended will be stored in this variable      
       
        //CONSTRUCTOR
        public TurnContext(string player)
        {
            this.player = player;            
        }        

        //GetplayerChoice Method
        public void GetPlayerChoice()
        {
            Console.WriteLine("");
            Console.WriteLine(this.player + " choose a choice using the number: Rock(1), Paper(2), Scissors(3), Lizard(4) or Spock(5)");
            string playerInput = Console.ReadLine();//get the player input
            int playerInputNumber;//variable to store the conversion from player input to int                                            
            while (!int.TryParse(playerInput, out playerInputNumber) || playerInputNumber < 1 || playerInputNumber > 5)//this loop will keep repeating until the player input a valid number to choose an option, once the number is valid it will already be referenced in playerChoiceNumber
            {
                Console.WriteLine("Invalid Choice. Please choose a number corresponding to your choice.");
                playerInput = Console.ReadLine();//reset the playerInput based on new input
            }
           //out of the loop convert the input number into a Choice enumerator option                      
           Choice playerChoice = (Choice)(playerInputNumber - 1);//using the user input number and subtracting 1 to get the correct index corresponding to his choice at Choice enumerator
           this.playerChoice = playerChoice;//assigning the correct value to playerChoice prop of the object instantiated                                    
        }

        //GetComputerChoice Method
        public void GenerateComputerChoice()
        {
            var random = new Random();//instatiating random object  
            int randomIndex = random.Next(0, 5);//generating random index from 0 to 4
            Choice computerChoice = (Choice)randomIndex;//choosing from Choice enumerator using random index
            this.computerChoice = computerChoice;//storing the computer choice
        }
        

        //TurnResult method
        public void GetTurnResult()
        {
            switch (this.playerChoice)
            {
                case Choice.Rock: //player chose Rock
                    //player wins
                    if (this.computerChoice == Choice.Lizard || this.computerChoice == Choice.Scissors)
                    {                        
                        Console.WriteLine(this.playerChoice + " crushes " + this.computerChoice + ". You won!");
                        this.turnResult = Result.Player;
                    }
                    //computer wins
                    else if (this.computerChoice == Choice.Spock || this.computerChoice == Choice.Paper)
                    {
                        string action = this.computerChoice == Choice.Spock ? " vaporizes " : " covers ";//checks the computer move to show the right action verb to the user
                        Console.WriteLine(this.computerChoice + action + this.playerChoice + ". You loose!");
                        this.turnResult = Result.Computer;
                    }
                    //draw
                    else
                    {
                        Console.WriteLine("It's a draw!");
                        this.turnResult = Result.Draw;
                    }
                    break;

                case Choice.Paper://player chose Paper
                    //player wins
                    if (this.computerChoice == Choice.Rock || this.computerChoice == Choice.Spock)
                    {
                        string action = this.computerChoice == Choice.Rock ? " covers " : " disproves ";//checks the computer move to show the right action verb to the user
                        Console.WriteLine(this.playerChoice + action + this.computerChoice + ". You won!");
                        this.turnResult = Result.Player;
                    }
                    //computer wins
                    else if (this.computerChoice == Choice.Scissors || this.computerChoice == Choice.Lizard)
                    {
                        string action = this.computerChoice == Choice.Scissors ? " cuts " : " eats ";//checks the computer move to show the right action verb to the user
                        Console.WriteLine(this.computerChoice + action + this.playerChoice + ". You loose!");
                        this.turnResult = Result.Computer;
                    }
                    //draw
                    else
                    {
                        Console.WriteLine("It's a draw!");
                        this.turnResult = Result.Draw;
                    }
                    break;

                case Choice.Scissors://player chose Scissors
                    //player wins
                    if (this.computerChoice == Choice.Paper || this.computerChoice == Choice.Lizard)
                    {
                        string action = this.computerChoice == Choice.Paper ? " cuts " : " decapitates ";//checks the computer move to show the right action verb to the user
                        Console.WriteLine(this.playerChoice + action + this.computerChoice + ". You won!");
                        this.turnResult = Result.Player;
                    }
                    //computer wins
                    else if (this.computerChoice == Choice.Rock || this.computerChoice == Choice.Spock)
                    {
                        string action = this.computerChoice == Choice.Rock ? " crushes " : " smashes ";//checks the computer move to show the right action verb to the user
                        Console.WriteLine(this.computerChoice + action + this.playerChoice + ". You loose!");
                        this.turnResult = Result.Computer;
                    }
                    //draw
                    else
                    {
                        Console.WriteLine("It's a draw!");
                        this.turnResult = Result.Draw;
                    }
                    break;

                case Choice.Lizard://player chose Lizard
                    //player wins
                    if (this.computerChoice == Choice.Spock || this.computerChoice == Choice.Paper)
                    {
                        string action = this.computerChoice == Choice.Spock ? " poisons " : " eats ";//checks the computer move to show the right action verb to the user
                        Console.WriteLine(this.playerChoice + action + this.computerChoice + ". You won!");
                        this.turnResult = Result.Player;
                    }
                    //computer wins
                    else if (this.computerChoice == Choice.Scissors || this.computerChoice == Choice.Rock)
                    {
                        string action = this.computerChoice == Choice.Scissors ? " decapitates " : " crushes ";//checks the computer move to show the right action verb to the user
                        Console.WriteLine(this.computerChoice + action + this.playerChoice + ". You loose!");
                        this.turnResult = Result.Computer;
                    }
                    //draw
                    else
                    {
                        Console.WriteLine("It's a draw!");
                        this.turnResult = Result.Draw;
                    }
                    break;

                case Choice.Spock://player chose Spock
                    //player wins
                    if (this.computerChoice == Choice.Scissors || this.computerChoice == Choice.Rock)
                    {
                        string action = this.computerChoice == Choice.Scissors ? " smashes " : " crushes ";//checks the computer move to show the right action verb to the user
                        Console.WriteLine(this.playerChoice + action + this.computerChoice + ". You won!");
                        this.turnResult = Result.Player;
                    }
                    //computer wins
                    else if (this.computerChoice == Choice.Lizard || this.computerChoice == Choice.Paper)
                    {
                        string action = this.computerChoice == Choice.Lizard ? " poisons " : " disproves ";//checks the computer move to show the right action verb to the user
                        Console.WriteLine(this.computerChoice + action + this.playerChoice + ". You loose!");
                        this.turnResult = Result.Computer;
                    }
                    //draw
                    else
                    {
                        Console.WriteLine("It's a draw!");
                        this.turnResult = Result.Draw;
                    }
                    break;
            }                        
        }

    }
}
