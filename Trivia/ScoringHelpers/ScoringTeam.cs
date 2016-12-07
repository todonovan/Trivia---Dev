using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaData.Models;
using System.Runtime.Serialization;

namespace Trivia.ScoringHelpers
{
    public class ScoringTeam
    {
        public Team Team { get; private set; }
        public int Score { get; private set; }
        public AnswerSet AnswerSet { get; private set; }
        public bool ScoreNeedsUpdated { get; private set; }
        private int _pointsPerQuestion;

        public ScoringTeam(Team t, int numRounds, int numQuestions, int pointsPerQuestion)
        {
            Team = t;
            Score = 0;
            AnswerSet = new AnswerSet(numRounds, numQuestions);
            ScoreNeedsUpdated = false;
            _pointsPerQuestion = pointsPerQuestion;
        }

        public void SetRoundAnswers(List<Question> answers, int roundNumber)
        {
            for (int i = 0; i < answers.Count; i++)
            {
                AnswerSet.SetAnswer(roundNumber, i, answers[i]);
            }
            ScoreNeedsUpdated = true;
        }
        
        public List<Question> GetRoundAnswers(int roundNumber)
        {
            List<Question> answers = AnswerSet.GetAnswersForRound(roundNumber);
            return answers;
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
                        if (answers[i][j] == Question.Correct) score += _pointsPerQuestion;
                        else if (answers[i][j] == Question.NotAnswered) score += (_pointsPerQuestion / 2);
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
