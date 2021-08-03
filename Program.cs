using System;
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
                Console.WriteLine("");
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
        //Decaring variables
        IApplicationRepository applicationRepository;
        int playerTurnWins;//global variable to track if a player reach 4 wins to finish the game
        int computerTurnWins;//global variable to track if a player reach 4 wins to finish the game
        public string  player;
        public int turnCount;//global variable to track the turns
        public List<Turn> gameTurns;//list with the turn objects for this game

        //CONSTRUCTOR
        public GameContext(string player)
        {
            this.player = player;
            this.playerTurnWins = 0;//resetting count
            this.computerTurnWins = 0;//resetting count
            this.gameTurns = new List<Turn>();
            this.turnCount = 0;//resetting turn count
            applicationRepository = new ApplicationRepository();
        }        

        //PlayGame method
        public void PlayGame()
        {            
            this.playerTurnWins = 0;//resetting count
            this.computerTurnWins = 0;//resetting count
            this.gameTurns = new List<Turn>();
            this.turnCount = 0;//resetting turn count
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
                    this.turnCount++;
                    Turn finishedTurn = newTurnContext.TurnResult(newTurn);//get the result the turn and update the turn object;
                    finishedTurn.GameID = game.GameID;//passing the GameID to the turn object
                    this.gameTurns.Add(finishedTurn);//add the finished turn to the list of turns of this game
                    incrementCounts(finishedTurn.TurnResult, game);                   
                    Console.WriteLine("This was turn number: " + this.turnCount);
                }
                else
                {
                    Console.WriteLine("This option is not valid please choose again.");
                }               
            }

            FinishGame(game);
                        
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
            Console.WriteLine("Game summary:");
            Console.WriteLine("You won " + game.PlayerTurnWins + " turns");
            Console.WriteLine("Computer won " + game.ComputerTurnWins + " turns");
            Console.WriteLine("There were " + game.TurnDraws + " draws");
            Console.WriteLine("There were a total of " + this.gameTurns.Count() + " turns");
            Console.WriteLine("");

            if (game.PlayerTurnWins > game.ComputerTurnWins)
            {
                Console.WriteLine("You've won the game!");
                game.GameResult = "Player won";
            }
            else
            {
                Console.WriteLine("Computer has won the game!");
                game.GameResult = "Computer won";
            }
            game.GameEndTime = DateTime.Now;
            applicationRepository.InsertGameInDatabase(game);
            foreach(var turn in this.gameTurns)
            {
                applicationRepository.InsertTurnInDatabase(turn);//iterate through the turns of this game and add them to database
            }

            PlayAgain();
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


    //Turn class
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
