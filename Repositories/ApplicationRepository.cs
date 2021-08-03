using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RockPaperScissors.Models;

namespace RockPaperScissors.Repositories
{
    public interface IApplicationRepository
    {
        void InsertTurnsInDatabase(List<TurnModel> turns);
        void InsertGameInDatabase(GameModel game, List<TurnModel> turns);
        int GetLatestGameID();        
    }
    public class ApplicationRepository : IApplicationRepository
    {
        RockPaperScissorsDbContext db;        
        public ApplicationRepository()
        {
            db = new RockPaperScissorsDbContext();
        }

        public void InsertTurnsInDatabase(List<TurnModel> turns)
        {
            db.Turns.AddRange(turns); 
            db.SaveChanges();
        }
        public void InsertGameInDatabase(GameModel game, List<TurnModel> turns)
        {
            db.Games.Add(game);
            InsertTurnsInDatabase(turns);
            db.SaveChanges();
        }
        public int GetLatestGameID()
        {
            int gamesTableCount = db.Games.ToList().Count();
            if (gamesTableCount != 0)//get the latest id if the table has any rows
            {
                int latestGameID = db.Games.Select(temp => temp.GameID).Max();
                return latestGameID; 
            }
            else //return 0 if the table is empty
            {
                return 0;
            }
        }        
    }
}
