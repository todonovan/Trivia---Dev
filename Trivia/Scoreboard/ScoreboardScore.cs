using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivia.Scoreboard
{
    public class ScoreboardScore : BindableBase
    {
        public string Name { get; set; }
        public int Score { get; set; }
    }
}
