using System.Data.Entity;

namespace RockPaperScissors.Models
{
    public class RockPaperScissorsDbContext : DbContext
    {
        //Constructor with the connection string defined in App.config
        //Please replace the Data Source attribute of the connection string to your own loal server at App.config
        //The .sql file used to create the database is located in the sql folder of the project
        public RockPaperScissorsDbContext() : base("RockPaperScissorsDbConn")
        {
         
        }

        public DbSet<Turn> Turns { get; set; }
        public DbSet<Game> Games { get; set; }
    }
}
