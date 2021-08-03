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
        IApplicationRepository applicationRepository;
        int playerTurnWins;//global variable to track if a player reach 4 wins to finish the game
        int computerTurnWins;//global variable to track if a player reach 4 wins to finish the game
        public string player;        
        public List<Turn> gameTurns;//list with the turn objects for this game

        //CONSTRUCTOR
        public GameContext(string player)
        {
            this.player = player;
            this.playerTurnWins = 0;//resetting count
            this.computerTurnWins = 0;//resetting count
            this.gameTurns = new List<Turn>();            
            applicationRepository = new ApplicationRepository();
        }

        //PlayGame method
        public void PlayGame()
        {
            //resetting variables again in case the player choose to play again at the end of a game
            this.playerTurnWins = 0;//resetting count
            this.computerTurnWins = 0;//resetting count
            this.gameTurns = new List<Turn>();//resetting turns list            

            Game game = new Game();//instantiating object from Game model class            
            game.PlayerTurnWins = 0;//resetting count
            game.ComputerTurnWins = 0;//resetting count
            game.TurnDraws = 0;//resetting count  
            game.PlayerName = this.player;
            game.GameID = applicationRepository.GetLatestGameID() + 1;//get lastest gameID and add 1 to be next ID

            while (this.playerTurnWins < 4 && this.computerTurnWins < 4)//keep playing turns until one player reach 4 turn wins
            {
                TurnContext newTurnContext = new TurnContext(game.PlayerName);
                Turn newTurn = newTurnContext.PlayTurn();

                if (newTurn.PlayerChoice == "rock" || newTurn.PlayerChoice == "paper" || newTurn.PlayerChoice == "scissors" || newTurn.PlayerChoice == "lizard" || newTurn.PlayerChoice == "spock")
                {
                    newTurn.ComputerChoice = GenerateComputerChoice(); ;
                    Console.WriteLine("You chose: " + newTurn.PlayerChoice);
                    Console.WriteLine("Computer chose: " + newTurn.ComputerChoice);                    
                    Turn finishedTurn = newTurnContext.TurnResult(newTurn);//get the result the turn and update the turn object;
                    finishedTurn.GameID = game.GameID;//passing the GameID to the turn object
                    this.gameTurns.Add(finishedTurn);//add the finished turn to the list of turns of this game
                    incrementCounts(finishedTurn.TurnResult, game);
                    Console.WriteLine("This was turn number: " + this.gameTurns.Count());
                }
                else
                {
                    Console.WriteLine("This option is not valid please choose again.");
                }
            }

            FinishGame(game);//Finish game once a player reach 4 wins and the while loop is interrupted

        }


        //incrementCounts Method
        public void incrementCounts(string result, Game game)
        {
            if (result == "Player won")
            {
                game.PlayerTurnWins++;//increment game object property
                this.playerTurnWins++;//increment class property to track a winner
            }
            else if (result == "Computer won")
            {
                game.ComputerTurnWins++;//increment game object property
                this.computerTurnWins++;//increment class property to track a winner
            }
            else
            {
                game.TurnDraws++;//increment game object property
            }

            Console.WriteLine("You won: " + game.PlayerTurnWins);
            Console.WriteLine("Computer won: " + game.ComputerTurnWins);
            Console.WriteLine("Draws: " + game.TurnDraws);
        }


        //FinishGame Method
        public void FinishGame(Game game)
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("GAME SUMMARY:");
            Console.WriteLine("You won " + game.PlayerTurnWins + " turns");
            Console.WriteLine("Computer won " + game.ComputerTurnWins + " turns");
            Console.WriteLine("There were " + game.TurnDraws + " draws");
            Console.WriteLine("There were a total of " + this.gameTurns.Count() + " turns");
            DisplayPlayerMostUsed(this.gameTurns);
            DisplayComputerMostUsed(this.gameTurns);
            Console.WriteLine("");

            if (game.PlayerTurnWins > game.ComputerTurnWins)
            {
                Console.WriteLine("Congratilations! You've won the game!");
                game.GameResult = "Player won";
            }
            else
            {
                Console.WriteLine("Oops! Computer has won the game!");
                game.GameResult = "Computer won";
            }

            game.GameEndTime = DateTime.Now;

            applicationRepository.InsertGameInDatabase(game);//calling repository to add the game to database
            foreach (var turn in this.gameTurns)
            {
                applicationRepository.InsertTurnInDatabase(turn);//iterate through the turns of this game and add them to database
            }

            PlayAgain();
        }

        public void DisplayComputerMostUsed(List<Turn> turns)
        {
            List<String> computerMoves = new List<String>();

            foreach (var turn in turns)
            {
                computerMoves.Add(turn.ComputerChoice);
            }

            var computerMoveGroup = computerMoves.GroupBy(x => x);
            var computerMaxCount = computerMoveGroup.Max(g => g.Count());
            var computerMostCommons = computerMoveGroup.Where(x => x.Count() == computerMaxCount).Select(x => x.Key).ToArray();

            Console.WriteLine("");
            Console.WriteLine("Computer most used choice(s):");
            foreach (var usedChoice in computerMostCommons)
            {
                Console.WriteLine(usedChoice);
            }
            Console.WriteLine("Used " + computerMaxCount + " time(s)");
        }

        public void DisplayPlayerMostUsed(List<Turn> turns)
        {
            List<String> playerMoves = new List<String>();

            foreach (var turn in turns)
            {
                playerMoves.Add(turn.PlayerChoice);
            }

            var playerMoveGroup = playerMoves.GroupBy(x => x);
            var playerMaxCount = playerMoveGroup.Max(g => g.Count());
            var playerMostCommons = playerMoveGroup.Where(x => x.Count() == playerMaxCount).Select(x => x.Key).ToArray();

            Console.WriteLine("");
            Console.WriteLine("Your most used choice(s):");
            foreach (var usedChoice in playerMostCommons)
            {
                Console.WriteLine(usedChoice);
            }
            Console.WriteLine("Used " + playerMaxCount + " time(s)");
        }



        //PlayAgain method
        public void PlayAgain()
        {
            Console.WriteLine("");
            Console.WriteLine("Type 'y' and hit enter to play again");
            string answer = Console.ReadLine();
            if (answer == "y")
            {
                this.PlayGame();
            }
            else
            {
                Console.WriteLine("");
                Console.Write("Thank you for playing!");
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
}
