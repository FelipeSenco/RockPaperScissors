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
        string gameResult;//variable to store the game result
        public string player;        
        public List<Turn> gameTurns;//list to store the turn objects for this game

        //CONSTRUCTOR
        public GameContext(string player)
        {
            this.player = player;
            this.playerTurnWins = 0;//resetting count
            this.computerTurnWins = 0;//resetting count
            this.turnDraws = 0;//resetting count
            this.gameTurns = new List<Turn>();                        
        }

        //PlayGame method
        public bool PlayGame()
        {                                                       
            while (this.playerTurnWins < 4 && this.computerTurnWins < 4)//keep playing turns until one player reach 4 turn wins
            {
                TurnContext currentTurn = new TurnContext(this.player);//creating an objec for the turn context  class              
                currentTurn.GetPlayerChoice();//get the player move choice

                if (currentTurn.playerChoice == "rock" || currentTurn.playerChoice == "paper" || currentTurn.playerChoice == "scissors" || currentTurn.playerChoice == "lizard" || currentTurn.playerChoice == "spock")
                {
                    Console.WriteLine("Turn Number: " + (this.gameTurns.Count() + 1));//get the current turn number by adding 1 to the list of turns                    

                    currentTurn.computerChoice = GenerateComputerChoice();

                    Console.WriteLine("You chose: " + currentTurn.playerChoice);
                    Console.WriteLine("Computer chose: " + currentTurn.computerChoice);

                    currentTurn.GetTurnResult();//get the result the turn from TurnContext.cs method;
                                                //
                    IncrementCounts(currentTurn.turnResult);//calling IncrementAndDisplayCounts to update values

                    Console.WriteLine("You won: " + this.playerTurnWins);
                    Console.WriteLine("Computer won: " + this.computerTurnWins);
                    Console.WriteLine("Draws: " + this.turnDraws);

                    AddTurnsToList(currentTurn); //add the current turn to the list of Turn objects that will be sent to database  after the game ends           
                }
                else
                {
                    Console.WriteLine("This option is not valid please choose again.");
                }
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

            return FinishGame();//Finish game once a player reach 4 wins and the while loop is interrupted

        }
        
        //incrementCounts Method
        public void IncrementCounts(string result)
        {
            if (result == "Player won")
            {                
                this.playerTurnWins++;//increment class property to track a winner
            }
            else if (result == "Computer won")
            {                
                this.computerTurnWins++;//increment class property to track a winner
            }
            else
            {
                this.turnDraws++;//increment game object property
            }
            
        }


        //FinishGame Method
        public bool FinishGame()
        {
            
            if (this.playerTurnWins > this.computerTurnWins)
            {
                Console.WriteLine("Congratilations! You've won the game!");
                this.gameResult = "Player won";
            }
            else
            {
                Console.WriteLine("Oops! Computer has won the game!");
                this.gameResult = "Computer won";
            }

            AddGameToDatabase();//calling method to insert the game data and its turns in the database
            return PlayAgain();
        }

        public void DisplayMostUsedMoves(List<Turn> turns, bool isPlayer = true)
        {
            List<String> moves = new List<String>(); //list to store the moves of the game

            foreach (var turn in turns)//iterate through the turn list for this game
            {
                string choice;
                choice = isPlayer ? turn.PlayerChoice : turn.ComputerChoice; //check to see isPlayer                               
                moves.Add(choice); //ad move to moves list
            }

            var moveGroup = moves.GroupBy(x => x);
            var maxCount = moveGroup.Max(g => g.Count());
            var mostCommons = moveGroup.Where(x => x.Count() == maxCount).Select(x => x.Key).ToArray();

            Console.WriteLine("");
            string message = isPlayer ? "Your most used choice(s):" : "Computer most used choice(s):";
            Console.WriteLine(message);
            foreach (var usedChoice in mostCommons)
            {
                Console.WriteLine(usedChoice);
            }
            Console.WriteLine("Used " + maxCount + " time(s)");
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

        public void AddTurnsToList(TurnContext currentTurn)//this method will pass the TurnContext object properties to a TurnModel object that will be stored in a list for further insertion at databse
        {
            IApplicationRepository repository = new ApplicationRepository();//instantiating a repository to get the current gameID using getLatestGameID();
            
            Turn turn = new Turn();
            turn.GameID = repository.GetLatestGameID() + 1;
            turn.PlayerName = currentTurn.player;
            turn.PlayerChoice = currentTurn.playerChoice;
            turn.ComputerChoice = currentTurn.computerChoice;
            turn.TurnResult = currentTurn.turnResult;
            turn.TurnEndTime = DateTime.Now;            
            this.gameTurns.Add(turn);//finally add the turn to list that will be accessed at AddGameToDatabase to add the turns of the game to the turns table
        }

        public void AddGameToDatabase() //method to call the repository and pass the game and turns models to be added at their respective table at database
        {            
            Game game = new Game();//creating an object of game model to send to repository
            game.PlayerName = this.player;
            game.PlayerTurnWins = this.playerTurnWins;
            game.ComputerTurnWins = this.computerTurnWins;
            game.TurnDraws = this.turnDraws;
            game.GameResult = this.gameResult;
            game.GameEndTime = DateTime.Now;

            IApplicationRepository repository = new ApplicationRepository();//creating an instance of repository to call database related method
            repository.InsertGameInDatabase(game, this.gameTurns);
        }


        string GenerateComputerChoice()
        {
            var randomFactor = new Random();
            var possibleChoices = new List<string> { "rock", "paper", "scissors", "lizard", "spock" };
            int randomIndex = randomFactor.Next(possibleChoices.Count);
            return possibleChoices[randomIndex];
        }
    }
}
