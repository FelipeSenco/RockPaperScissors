using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.Entity;

namespace RockPaperScissors.Models
{
    public class RockPaperScissorsDbContext : DbContext
    {
        //Constructor with the connection string defined in App.config
        //Please replace the Data Source property of the connection string to your own loal server at App.config
        //The .sql file used to create the database is in the project folder
        public RockPaperScissorsDbContext() : base("RockPaperScissorsDbConn")
        {
         
        }

        public DbSet<Turn> Turns { get; set; }
        public DbSet<Game> Games { get; set; }

    }
}
