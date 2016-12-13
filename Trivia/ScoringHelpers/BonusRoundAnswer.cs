using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivia.ScoringHelpers
{
    public class BonusRoundAnswer
    {
        public int Wager { get; set; }
        public Question Answer { get; set; }
        public int BonusRoundNumber { get; set; }

        public BonusRoundAnswer(int wager, Question answer, int bonusRoundNumber)
        {
            Wager = wager;
            Answer = answer;
            BonusRoundNumber = bonusRoundNumber;
        }
    }
}
