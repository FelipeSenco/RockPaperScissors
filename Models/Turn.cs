using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RockPaperScissors.Models
{
    public class Turn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TurnID { get; set; }

        public string PlayerName { get; set; }

        public string PlayerChoice { get; set; }

        public string ComputerChoice { get; set; }

        public string TurnResult { get; set; }

        public DateTime TurnEndTime { get; set; }
        
        public int GameID { get; set; }        
    }
}
