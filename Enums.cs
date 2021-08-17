using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockPaperScissors
{
    public enum Choice//Enumerator to use the choices in a strongly typed way instead of using strings that are prone to typing errors
    {
        Rock,
        Paper,
        Scissors,
        Lizard,
        Spock
    }

    public enum Result//enumerator to acces the winner in a strongly typed manner, avoiding typing errors
    {
        Player,// for player wins
        Computer,//for computer wins
        Draw//for draws
    }
}
