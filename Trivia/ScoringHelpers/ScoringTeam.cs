using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaData.Models;
using System.Runtime.Serialization;

namespace Trivia.ScoringHelpers
{
    [DataContract()]
    public class ScoringTeam
    {
        [DataMember()]
        public Team Team { get; private set; }
        [DataMember()]
        public int Score { get; private set; }
        [DataMember()]
        public AnswerSet AnswerSet { get; private set; }
        public bool ScoreNeedsUpdated { get; private set; }
        private List<int> _roundPointVals;

        public ScoringTeam(Team t, int numRounds, int numQuestions, List<int> roundPointVals)
        {
            Team = t;
            Score = 0;
            AnswerSet = new AnswerSet(numRounds, numQuestions);
            ScoreNeedsUpdated = false;
            _roundPointVals = roundPointVals;
        }

        public void SetRoundAnswers(List<Question> answers, int roundNumber)
        {
            for (int i = 0; i < answers.Count; i++)
            {
                AnswerSet.SetAnswer(roundNumber, i, answers[i]);
            }
            ScoreNeedsUpdated = true;
        }

        public int GetScore()
        {
            if (ScoreNeedsUpdated)
            {
                int score = 0;
                var answers = AnswerSet.GetAllAnswers();
                for (int i = 0; i < answers.Count; i++)
                {
                    for (int j = 0; j < answers[i].Count; j++)
                    {
                        if (answers[i][j] == Question.Correct) score += _roundPointVals[i];
                        else if (answers[i][j] == Question.Incorrect) score -= _roundPointVals[i]; // are negative scores legal?
                    }
                }
                Score = score;
                ScoreNeedsUpdated = false;
                return score;
            }
            else return Score;
        }

        public bool HasRoundBeenScored(int roundNumber)
        {
            var roundAnswers = AnswerSet.GetAnswersForRound(roundNumber);
            foreach (var a in roundAnswers)
            {
                if (a == Question.NotJudged) return false;
            }
            return true;
        }
    }
}
