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
        void InsertTurnsInDatabase(List<Turn> turns);
        void InsertGameInDatabase(Game game, List<Turn> turns);
        int GetLatestGameID();        
    }
    public class ApplicationRepository : IApplicationRepository
    {
        RockPaperScissorsDbContext db;  
        
        //Constructor
        public ApplicationRepository()
        {
            db = new RockPaperScissorsDbContext();//instatiating the db context
        }


        //InsertTurnsInDatabase Method
        public void InsertTurnsInDatabase(List<Turn> turns)
        {
            db.Turns.AddRange(turns); 
            db.SaveChanges();
        }

        //InsertGameInDatabase Method
        public void InsertGameInDatabase(Game game, List<Turn> turns)
        {
            db.Games.Add(game);            
            db.SaveChanges();

            InsertTurnsInDatabase(turns);//calling InsertTurnsInDatabase here as all games must have their turns also stored at turns table
        }

        //GetLatestGameID Method
        public int GetLatestGameID()
        {
            int gamesTableCount = db.Games.ToList().Count();//getting rows from games able
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
