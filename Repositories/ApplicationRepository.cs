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
        void InsertTurnInDatabase(Turn turn);
        void InsertGameInDatabase(Game game);
        int GetLatestGameID();        
    }
    public class ApplicationRepository : IApplicationRepository
    {
        RockPaperScissorsDbContext db;        
        public ApplicationRepository()
        {
            db = new RockPaperScissorsDbContext();
        }

        public void InsertTurnInDatabase(Turn turn)
        {
            db.Turns.Add(turn);
            db.SaveChanges();
        }
        public void InsertGameInDatabase(Game game)
        {
            db.Games.Add(game);
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
