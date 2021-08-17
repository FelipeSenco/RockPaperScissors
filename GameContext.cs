using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RockPaperScissors.Models;
using RockPaperScissors.Services;

namespace RockPaperScissors
{
    public class GameContext
    {
        //Decaring variables        
        public int playerTurnWins;//global variable to track if a player reach 4 wins to finish the game
        public int computerTurnWins;//global variable to track if a player reach 4 wins to finish the game
        public int turnDraws;//global variable to track turn draws
        public Result gameResult;//variable to store the game result form the Result enumerator type (defined in TurnContext class)
        public string player;
        public List<TurnContext> gameTurns;//list to store the turn objects for this game       

        //CONSTRUCTOR
        public GameContext(string player)
        {
            this.player = player;
            this.playerTurnWins = 0;//resetting count
            this.computerTurnWins = 0;//resetting count
            this.turnDraws = 0;//resetting count
            this.gameTurns = new List<TurnContext>();//creating an empty list that will store the turns object from TurnContext.cs from this game  for further insertion in database                                
        }

        //PlayGame method
        public bool PlayGame()
        {                                                       
            while (this.playerTurnWins < 4 && this.computerTurnWins < 4)//keep playing turns until one player reach 4 turn wins
            {
                TurnContext currentTurn = new TurnContext(this.player);//creating an objec for the turn context  class              
                currentTurn.GetPlayerChoice();//get the player move
                currentTurn.GenerateComputerChoice();//get the computer move

                Console.WriteLine("");
                Console.WriteLine("Turn Number: " + (this.gameTurns.Count() + 1));//get the current turn number by adding 1 to the list of turns                    
                Console.WriteLine("You chose: " + currentTurn.playerChoice);
                Console.WriteLine("Computer chose: " + currentTurn.computerChoice);

                currentTurn.GetTurnResult();//get the result the turn from TurnContext.cs method;
                                                //
                IncrementCounts(currentTurn.turnResult);//calling IncrementAndDisplayCounts to update values

                Console.WriteLine("");
                Console.WriteLine("You won: " + this.playerTurnWins + " turn(s)");
                Console.WriteLine("Computer won: " + this.computerTurnWins + " turn(s)");
                Console.WriteLine("Draws: " + this.turnDraws);

                currentTurn.turnEndTime = DateTime.Now;
                AddTurnToList(currentTurn); //add the current turn to the list of Turn objects that will be sent to database  after the game ends
                
            }
            Console.WriteLine("");
            Console.WriteLine("The game has ended, press any key to see the result");
            Console.ReadLine();
            
            //interactions after leaving the game loop
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("GAME SUMMARY:");
            Console.WriteLine("You won " + this.playerTurnWins + " turns");
            Console.WriteLine("Computer won " + this.computerTurnWins + " turns");
            Console.WriteLine("There were " + this.turnDraws + " draws");
            Console.WriteLine("There were a total of " + this.gameTurns.Count() + " turns");
            DisplayMostUsedMoves(this.gameTurns);//displays the player most used moves
            DisplayMostUsedMoves(this.gameTurns, false);//display the computer most used moves as isPlayer argument is false
            Console.WriteLine("");

            return FinishGame();//Finish game once a player reach 4 wins and the while loop is interrupted. Return is used to get the bool value that will be used at Program.cs to decide if another game will be played
        }
        
        //incrementCounts Method
        public void IncrementCounts(Result result)
        {
            if (result == Result.Player)//check is the result is the same value as Player at the Result enum defined in TurnContext
            {                
                this.playerTurnWins++;//increment class property to track a winner
            }
            else if (result == Result.Computer)//check is the result is the same value as Computer at the Result enum defined in TurnContext
            {                
                this.computerTurnWins++;//increment class property to track a winner
            }
            else//In this case it is logically a draw
            {
                this.turnDraws++;//increment game object property
            }
            
        }


        //FinishGame Method
        public bool FinishGame()
        {
            
            if (this.playerTurnWins > this.computerTurnWins)//player win
            {
                Console.WriteLine("Congratulations! You've won the game!");

                this.gameResult = Result.Player;
            }
            else//computer win
            {
                Console.WriteLine("Computer has won the game!");

                this.gameResult = Result.Computer;
            }

            SendGameToService();//calling method to call the service 
            return PlayAgain();//returning Play Again and getting bool value depending on user input
        }

        //PlayAgain method
        public bool PlayAgain()
        {
            Console.WriteLine("");
            Console.WriteLine("Type 'y' and hit enter to play again");

            string answer = Console.ReadLine();

            if (answer == "y")
            {
                return true;
            }
            else
            {
                Console.WriteLine("");
                Console.Write("Thank you for playing!");

                return false;
            }
        }

        //DisplayMostUsedMoves Method
        public void DisplayMostUsedMoves(List<TurnContext> turns, bool isPlayer = true)
        {
            List<String> moves = new List<String>(); //list to store the moves of the game

            foreach (var turn in turns)//iterate through the turn list for this game
            {
                string choice;
                choice = isPlayer ? Convert.ToString(turn.playerChoice) : Convert.ToString(turn.computerChoice); //check to see isPlayer = true or not and use playerChoice or computerChoice depending on condition                        
                moves.Add(choice); //add move to moves list
            }

            var moveGroup = moves.GroupBy(x => x);//ordering the moves list
            var maxCount = moveGroup.Max(g => g.Count());//getting highest counters
            var mostCommons = moveGroup.Where(x => x.Count() == maxCount).Select(x => x.Key).ToArray();//selecting the values with higher counters

            Console.WriteLine("");

            string message = isPlayer ? "Your most used choice(s):" : "Computer most used choice(s):";//choosing which message to show based on isPlayer

            Console.WriteLine(message);

            foreach (var usedChoice in mostCommons)//iterate through mostCommons and outputting to the user
            {
                Console.WriteLine(usedChoice + "(" + maxCount + " time(s))");
            }            
        }        
        

        //AddTurnsToList Method
        public void AddTurnToList(TurnContext currentTurn)//this method will pass the TurnContext object to the list of turns for this game
        {                         
            this.gameTurns.Add(currentTurn);//
        }

        //AddGameToDatabase Method
        public void SendGameToService() //method to call the service and pass the gamecontext model and the list of turncontext from the game
        {
            ApplicationService applicationService = new ApplicationService();
            applicationService.InsertGame(this);
            applicationService.InsertTurns(this.gameTurns);
            
        }
              
    }
}
