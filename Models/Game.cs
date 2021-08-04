using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RockPaperScissors.Models
{
    public class Game
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GameID { get; set; }

        public string PlayerName { get; set; }
        public int PlayerTurnWins { get; set; }
        public int ComputerTurnWins { get; set; }
        public int TurnDraws { get; set; }

        public string GameResult { get; set; }

        public DateTime GameEndTime { get; set; }

        
    }
}
