using System.Collections.Generic;
using TriviaData.Models;
using System.Runtime.Serialization;
using Trivia.ScoringHelpers;

namespace Trivia.ScoringHelpers
{
    [DataContract()]
    public class ActiveScorer
    {
        [DataMember()]
        public Scorer Scorer { get; private set; }
        [DataMember()]
        public List<ScoringTeam> ScoringTeams { get; private set; }
        public string Name
        {
            get { return Scorer.Name; }
            private set { }
        }
        public List<int> RoundPointVals { get; private set; }

        public ActiveScorer(Scorer s, int numRounds, int numQuestions, List<int> roundPointVals)
        {
            Scorer = s;
            ScoringTeams = new List<ScoringTeam>();
            RoundPointVals = roundPointVals;
            foreach (var t in s.Teams) ScoringTeams.Add(new ScoringTeam(t, numRounds, numQuestions, RoundPointVals));
        }

        public Dictionary<string, int> ReportScores()
        {
            Dictionary<string, int> scores = new Dictionary<string, int>();
            foreach (var s in ScoringTeams)
            {
                scores.Add(s.Team.Name, s.GetScore());
            }
            return scores;
        }

        public bool AllTeamsScoredForRound(int roundNumber)
        {
            foreach (var t in ScoringTeams)
            {
                if (!t.HasRoundBeenScored(roundNumber)) return false;
            }
            return true;
        }
    }
}
