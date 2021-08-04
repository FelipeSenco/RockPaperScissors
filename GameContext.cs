using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RockPaperScissors.Models;
using RockPaperScissors.Repositories;

namespace RockPaperScissors
{
    public class GameContext
    {
        //Decaring variables        
        int playerTurnWins;//global variable to track if a player reach 4 wins to finish the game
        int computerTurnWins;//global variable to track if a player reach 4 wins to finish the game
        int turnDraws;//global variable to track turn draws
        Result gameResult;//variable to store the game result form the Result enumerator type (defined in TurnContext class)
        public string player;   
        public List<Turn> gameTurns;//list to store the turn objects for this game

        //CONSTRUCTOR
        public GameContext(string player)
        {
            this.player = player;
            this.playerTurnWins = 0;//resetting count
            this.computerTurnWins = 0;//resetting count
            this.turnDraws = 0;//resetting count
            this.gameTurns = new List<Turn>();//creating an empty list that will store the turns object from Turn.cs from this game  for further insertion in database                    
        }

        //PlayGame method
        public bool PlayGame()
        {                                                       
            while (this.playerTurnWins < 4 && this.computerTurnWins < 4)//keep playing turns until one player reach 4 turn wins
            {
                TurnContext currentTurn = new TurnContext(this.player);//creating an objec for the turn context  class              
                currentTurn.GetPlayerChoice();//get the player move
                currentTurn.GenerateComputerChoice();//get the computer move
                
                    Console.WriteLine("Turn Number: " + (this.gameTurns.Count() + 1));//get the current turn number by adding 1 to the list of turns                    

                    Console.WriteLine("You chose: " + currentTurn.playerChoice);
                    Console.WriteLine("Computer chose: " + currentTurn.computerChoice);

                    currentTurn.GetTurnResult();//get the result the turn from TurnContext.cs method;
                                                //
                    IncrementCounts(currentTurn.turnResult);//calling IncrementAndDisplayCounts to update values

                    Console.WriteLine("You won: " + this.playerTurnWins);
                    Console.WriteLine("Computer won: " + this.computerTurnWins);
                    Console.WriteLine("Draws: " + this.turnDraws);

                    AddTurnToList(currentTurn); //add the current turn to the list of Turn objects that will be sent to database  after the game ends
                
            }

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
                Console.WriteLine("Congratilations! You've won the game!");

                this.gameResult = Result.Player;
            }
            else//computer win
            {
                Console.WriteLine("Oops! Computer has won the game!");

                this.gameResult = Result.Computer;
            }

            AddGameToDatabase();//calling method to insert the game data and its turns in the database
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
        public void DisplayMostUsedMoves(List<Turn> turns, bool isPlayer = true)
        {
            List<String> moves = new List<String>(); //list to store the moves of the game

            foreach (var turn in turns)//iterate through the turn list for this game
            {
                string choice;
                choice = isPlayer ? turn.PlayerChoice : turn.ComputerChoice; //check to see isPlayer = true or not and use playerChoice or computerChoice depending on condition                        
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
                Console.WriteLine(usedChoice);
            }
            Console.WriteLine("Used " + maxCount + " time(s)");
        }        
        

        //AddTurnsToList Method
        public void AddTurnToList(TurnContext currentTurn)//this method will pass the TurnContext object properties to a TurnModel object that will be stored in a list for further insertion at databse
        {
            IApplicationRepository repository = new ApplicationRepository();//instantiating a repository to get the current gameID using getLatestGameID();
            
            Turn turn = new Turn();
            turn.GameID = repository.GetLatestGameID() + 1;//setting the correct GameID for this turn
            turn.PlayerName = currentTurn.player;
            turn.PlayerChoice = Convert.ToString(currentTurn.playerChoice);//converting the choice from the enumerator into string for database storage
            turn.ComputerChoice = Convert.ToString(currentTurn.computerChoice);//converting the choice from the enumerator into string for database storage
            turn.TurnResult = Convert.ToString(currentTurn.turnResult);//converting the result from the enumerator into string for database storage
            turn.TurnEndTime = DateTime.Now;            
            this.gameTurns.Add(turn);//finally add the turn to list that will be accessed at AddGameToDatabase to add the turns of the game to the turns table
        }

        //AddGameToDatabase Method
        public void AddGameToDatabase() //method to call the repository and pass the game and turns models to be added at their respective table at database
        {            
            Game game = new Game();//creating an object of game model to send to repository
            game.PlayerName = this.player;
            game.PlayerTurnWins = this.playerTurnWins;
            game.ComputerTurnWins = this.computerTurnWins;
            game.TurnDraws = this.turnDraws;
            game.GameResult = Convert.ToString(this.gameResult) + " won";//converting the result from Result enumerator to string and " won" to storage at database
            game.GameEndTime = DateTime.Now;

            IApplicationRepository repository = new ApplicationRepository();//creating an instance of repository to call database related method
            repository.InsertGameInDatabase(game, this.gameTurns);
        }
              
    }
}
