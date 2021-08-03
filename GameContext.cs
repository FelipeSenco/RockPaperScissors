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
        public List<TurnModel> gameTurns;//list to store the turn objects for this game

        //CONSTRUCTOR
        public GameContext(string player)
        {
            this.player = player;
            this.playerTurnWins = 0;//resetting count
            this.computerTurnWins = 0;//resetting count
            this.turnDraws = 0;//resetting count
            this.gameTurns = new List<TurnModel>();                        
        }

        //PlayGame method
        public bool PlayGame()
        {                                                       
            while (this.playerTurnWins < 4 && this.computerTurnWins < 4)//keep playing turns until one player reach 4 turn wins
            {
                TurnContext currentTurnContext = new TurnContext(this.player);//creating an objec for the turn context
                TurnModel currentturn = currentTurnContext.PlayTurn();//creating an object for the turn model

                if (currentturn.PlayerChoice == "rock" || currentturn.PlayerChoice == "paper" || currentturn.PlayerChoice == "scissors" || currentturn.PlayerChoice == "lizard" || currentturn.PlayerChoice == "spock")
                {
                    currentturn.ComputerChoice = GenerateComputerChoice(); ;
                    Console.WriteLine("You chose: " + currentturn.PlayerChoice);
                    Console.WriteLine("Computer chose: " + currentturn.ComputerChoice);
                    string turnResult = currentTurnContext.TurnResult(currentturn);//get the result the turn and update the turn object;
                    //finishedTurn.GameID = game.GameID;//passing the GameID to the turn object
                    this.gameTurns.Add(currentturn);//add the finished turn to the list of turns of this game
                    IncrementCounts(turnResult);
                    Console.WriteLine("This was turn number: " + this.gameTurns.Count());
                }
                else
                {
                    Console.WriteLine("This option is not valid please choose again.");
                }
            }

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

            Console.WriteLine("You won: " + this.playerTurnWins);
            Console.WriteLine("Computer won: " + this.computerTurnWins);
            Console.WriteLine("Draws: " + this.turnDraws);
        }


        //FinishGame Method
        public bool FinishGame()
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("GAME SUMMARY:");
            Console.WriteLine("You won " + this.playerTurnWins + " turns");
            Console.WriteLine("Computer won " + this.computerTurnWins + " turns");
            Console.WriteLine("There were " + this.turnDraws + " draws");
            Console.WriteLine("There were a total of " + this.gameTurns.Count() + " turns");
            DisplayMostUsedMoves(this.gameTurns);
            DisplayMostUsedMoves(this.gameTurns, false);
            Console.WriteLine("");

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

            return PlayAgain();
        }

        public void DisplayMostUsedMoves(List<TurnModel> turns, bool isPlayer = true)
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

        public void AddGameToDatabase()
        {            
            GameModel game = new GameModel();//creating an object of game model to send to repository
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
