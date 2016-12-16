using System.Collections.Generic;
using System.Linq;
using Trivia.Scorers;
using System.Runtime.Serialization;
using Trivia.Scoreboard;

namespace Trivia.ScoringHelpers
{
    public class GameState
    {
        public int NumRounds { get; set; }
        public int NumQuestionsPerRound { get; set; }
        public int PointsPerQuestion { get; set; }
        public List<ActiveScorer> ActiveScorers { get; set; }
        public int NumberOfCompleteBonusRounds { get; set; }
        public string FileName { get; set; }

        public GameState()
        {
            ActiveScorers = new List<ActiveScorer>();
        }

        public GameState(int numRounds, int numQuestionsPerRound, int pointsPerQuestion, List<ActiveScorer> scorers, int numBonusRounds, string fileName)
        {
            NumRounds = numRounds;
            NumQuestionsPerRound = numQuestionsPerRound;
            PointsPerQuestion = pointsPerQuestion;
            ActiveScorers = scorers;
            NumberOfCompleteBonusRounds = numBonusRounds;
            FileName = fileName;
        }

        public List<ReportedScore> GetAllScores()
        {
            List<Dictionary<string, int>> scoresList = new List<Dictionary<string, int>>();
            foreach (var s in ActiveScorers)
            {
                scoresList.Add(s.ReportScores());
            }

            var scoresDict = scoresList.SelectMany(dict => dict).ToDictionary(score => score.Key, score => score.Value).ToList();
            List<ReportedScore> scores = new List<ReportedScore>();
            foreach (var pair in scoresDict)
            {
                ReportedScore s = new ReportedScore();
                s.TeamName = pair.Key;
                s.Score = pair.Value;
                scores.Add(s);
            }
            return scores;
        }
    }
}
