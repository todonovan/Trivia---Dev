using System.Collections.Generic;
using Trivia.Scorers;
using System.Runtime.Serialization;

namespace Trivia.ScoringHelpers
{
    public class GameState
    {
        public int NumRounds { get; set; }
        public int NumQuestionsPerRound { get; set; }
        public List<ActiveScorer> ActiveScorers { get; set; }
    }
}
