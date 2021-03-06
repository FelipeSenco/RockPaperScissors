# RockPaperScissors
C#.Net Console app to play Rock, Paper, Scissors with a twist: Lizard and Spock!
The games a stored in a Sql local database

Thr rules are: Rock beats Scissors and Lizard; Paper beats Rock and Spock; Scissors beats Paper and Lizard; Lizard beats Spock and Paper; Spock beats Rock and Scissors
Best of 7: the first to reach 4 turn wins is the winner

Application Flow:

===============================Program.cs==================================
Inside the main method, the initial outputs to the user are displayed
Get the player name through user input
Call the PlayGame() method of GameContext.cs if the gamePlaying variable is true



===============================GameContext.cs==================================

This is the class the handle the game loop and have logic to finish the game, it is responsible for calling the application repository to store data
Objects of this class will have properties: .player .playerTurnWins .computerTurnWins .turnDraws .gameTurns

------PlayGame()-----
This method will start the game by generating TurnContext objects while no player has reached 4 wins (it is best of 7)
At the end of each loop IncrementCounts() and AddTurnToList() methods are called
When a player reach 4 wins it will exit the While loop and log more info to the user
Call DisplayMostUsedMoves() for player and computer moves
Call FinishGame()

------IncrementCounts(result)------
This method checks who is the winner and increment .computerTurnWins, . playerTurnWins or .turnDraws depending on the winner

------FinishGame()------
This method checks if the player or the computer has more wins to output the result to the user
Store the game result at .gameResult property
Call AddGameToDataBase() method
Call PlayGain() method

------PlayAgain()------
Ask the user if another game should start
Returns true or false based on the user input


------DisplayMostUsedMoves(List of turns, isPlayer)------
This method has logic to retrieve the most used moves
It access the player most used methods if isPlayer = true, if it is false it access the computer most used moves instead

------AddTurnToList(current turn)------
This method will add the currentTurn instantiaded from TurnContext class to the gameTurns(from GameContext) list.


------SendGameToService()------
This method will instantiate an object from application service
It calls the InsertGame Method from the service with the gamecontext object itself as a parameter
It calls the InsertTurns method from the service with the list of TurnContexts objects from the gameContext object



===============================Game.cs==================================

This is the model class with properties corresponding with the database table Games:
.GameID .PlayerName .PlayerTurnWins .ComputerTurnWins .TurnDraws .GameResult .GameEndTime
This model class object is used to store Game data in the database




===============================TurnContext.cs==================================

In this namespace 2 Enumerators are defined, Choice and Result, to avoid typing erros when checking conditions
This class will handle the turns logic
Properties: .player .playerChoice .computerChoice .turnResult .turnEndTime

------GetPlayerChoice()------
This method will ask the user to input a choice of number, check if it is a valid choice
Get the corresponding choice from the Choice enumerator
Store the choice at .playerChoice property

------GetComputerChoice()------
This method will generate a random index
Get the choice from the Choice enumerator using the random index
Store the choice at .computerChoice property

------GetTurnResult()------
This method will check the turn winner based on the Game rules
Store the result at .turnResult property




===============================Turn.cs==================================

This is the model class with properties corresponding with the database table Turns:
.TurnID .PlayerName .PlayerChoice .ComputerChoice .TurnResult .GameID .TurnEndTime
This model class object is used to store Game data in the database




===============================RockPaperScissorsDbContext.cs==================================

This class inherits from DbContext (from entityFramework package)
At the constructor of the class it references the connection string defined att App.config to connect with Sql server
It defines DbSet<Turn> and DbSet<Game> with getters and setters for operations in database
  
  ****In order to run this application on your pc:****
MAKE SURE TO CHANGE THE 'DATA SOURCE' ATTRIBUTE OF THE CONNECTION STRING AT APP.CONFIG TO USE YOUR OWN LOCALSERVER
  

===============================ApplicationService.cs==================================
------IApplicationServicey------
This interface defines the methods that must be implemented by the ApplicationService class
  
------ApplicationService------
At the constructor a ApplicationRepository object is instantiated
This class implement the three methods declared at the interface:
  -GetLatestGameID
  -InsertGame
  -InsertTurns  
  
  
===============================ApplicationRepository.cs==================================
  
This is the repository that will use the RockPaperScissorsDbContext object and perform database operations

------IApplicationRepository------
This interface defines the methods that must be implemented by the ApplicationRepository class
  
------ApplicationRepository------
At the constructor a RockPaperScissorsDbContext object is instantiated
This class implement the three methods declared at the interface:
  -InsertTurnsInDatabase
  -InsertGameInDatabase
  -GetLatestGameID
  

===============================RockPaperScissorsCreateDatabase.sql==================================  
This is the query to generate the database with Games and Turns tables.
  
  
