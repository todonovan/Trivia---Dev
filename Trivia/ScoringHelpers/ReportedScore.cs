using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivia.ScoringHelpers
{
    public class ReportedScore : BindableBase
    {
        public string TeamName { get; set; }
        public int Score { get; set; }
    }
}
