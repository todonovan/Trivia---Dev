using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivia.Scoring
{
    public class TeamRoundScoringViewModel : BindableBase
    {
        public int NumQuestions { get; private set; }

        public TeamRoundScoringViewModel(int numQuestions)
        {
            NumQuestions = numQuestions;
        }
    }
}
