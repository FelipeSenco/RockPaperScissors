using RockPaperScissors.Models;
using RockPaperScissors.Repositories;
using System;
using System.Collections.Generic;

namespace RockPaperScissors.Services
{
    public interface IApplicationService
    {
        void InsertTurns(List<TurnContext> turns);
        void InsertGame(GameContext game);
        int GetLatestGameID();
    }
    public class ApplicationService : IApplicationService
    {
        IApplicationRepository applicationRepository;
        public ApplicationService()
        {
             this.applicationRepository = new ApplicationRepository();
        }

        public int GetLatestGameID()
        {
            int latestGameId = this.applicationRepository.GetLatestGameID();
            return latestGameId;
        }

        public void InsertTurns(List<TurnContext> turns)
        {
            List<Turn> turnModels = new List<Turn>();
            int turnID = GetLatestGameID();//getting latest game id and adding 1 to make sure the turns have the right gameID

            foreach(var turn in turns)//iterating the turncontext list to do the migration and populate the turnmodels list
            {
                Turn turnModel = new Turn(); //instating a turn model object that will be used to migrate the data from all the TurnContext objects from the list
                turnModel.PlayerName = turn.player;
                turnModel.PlayerChoice = Convert.ToString(turn.playerChoice);
                turnModel.ComputerChoice = Convert.ToString(turn.computerChoice);
                turnModel.TurnResult = Convert.ToString(turn.turnResult);
                turnModel.TurnEndTime = turn.turnEndTime;
                turnModel.GameID = turnID;

                turnModels.Add(turnModel);
            }

            applicationRepository.InsertTurnsInDatabase(turnModels);//calling the repository to insert all the turns from the finished game in the database
        }

        public void InsertGame(GameContext game)
        {
            Game gameModel = new Game();//creating an object of game model to send to migrate data from the gamecontext
            gameModel.PlayerName = game.player;
            gameModel.PlayerTurnWins = game.playerTurnWins;
            gameModel.ComputerTurnWins = game.computerTurnWins;
            gameModel.TurnDraws = game.turnDraws;
            gameModel.GameResult = Convert.ToString(game.gameResult) + " won";//converting the result from Result enumerator to string and " won" to storage at database
            gameModel.GameEndTime = DateTime.Now;

            applicationRepository.InsertGameInDatabase(gameModel);//calling the repository to insert the game in database
        }
    }
}
