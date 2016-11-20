using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;

namespace Trivia.ScoringHelpers
{
    [DataContract()]
    public class ScoringRound
    {
        [DataMember()]
        public int OrderOfRound { get; set; }
        [DataMember()]
        public bool IsBonusRound { get; set; }
        [DataMember()]
        public int NumQuestions { get; set; }
        [DataMember()]
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
