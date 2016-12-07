using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;

namespace Trivia.ScoringHelpers
{
    public class ScoringRound
    {
        public int OrderOfRound { get; set; }
        public bool IsBonusRound { get; set; }
        public int NumQuestions { get; set; }
        public int PointValue { get; set; }

        public ScoringRound(int roundOrder, bool isBonus, int numQuestions, int pointValue)
        {
            OrderOfRound = roundOrder;
            IsBonusRound = isBonus;
            NumQuestions = numQuestions;
            PointValue = pointValue;
        }
    }
}
