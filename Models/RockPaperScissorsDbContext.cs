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
        public RockPaperScissorsDbContext() : base("RockpaperScissorsDbConn")
        {
         
        }

        DbSet<Turn> Turns { get; set; }
        DbSet<Game> Games { get; set; }

    }
}
