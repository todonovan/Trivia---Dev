using System.Collections.Generic;
using Trivia.Scorers;
using System.Runtime.Serialization;

namespace Trivia.ScoringHelpers
{
    [DataContract()]
    public class GameState
    {
        [DataMember()]
        public int NumRounds { get; set; }
        [DataMember()]
        public int NumQuestionsPerRound { get; set; }
        [DataMember()]
        public List<ActiveScorer> ActiveScorers { get; set; }
    }
}
