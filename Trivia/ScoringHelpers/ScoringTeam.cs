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
        public List<BonusRoundAnswer> BonusRoundAnswers { get; private set; }
        public bool ScoreNeedsUpdated { get; private set; }
        private int _pointsPerQuestion;

        public ScoringTeam(Team t, int numRounds, int numQuestions, int pointsPerQuestion)
        {
            Team = t;
            Score = 0;
            AnswerSet = new AnswerSet(numRounds, numQuestions);
            ScoreNeedsUpdated = false;
            BonusRoundAnswers = new List<BonusRoundAnswer>();
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

        public void SetAllAnswers(List<List<Question>> answers)
        {
            for (int i = 0; i < answers.Count; i++)
            {
                SetRoundAnswers(answers[i], i);
            }
        }

        public List<List<Question>> GetAllNonBonusAnswers()
        {
            return AnswerSet.GetNonBonusAnswers();
        }

        public void SetBonusRoundAnswer(BonusRoundAnswer ans)
        {
            if (ans.BonusRoundNumber == BonusRoundAnswers.Count)
            {
                BonusRoundAnswers.Add(ans);
            }
            else if (ans.BonusRoundNumber < BonusRoundAnswers.Count)
            {
                BonusRoundAnswers[ans.BonusRoundNumber] = ans;
            }
            else
            {
                throw new ArgumentOutOfRangeException("BonusRoundAnswers", "Round number higher than expected");
            }
            ScoreNeedsUpdated = true;
        }
        
        public List<Question> GetNonBonusRoundAnswers(int roundNumber)
        {
            List<Question> answers = AnswerSet.GetAnswersForRound(roundNumber);
            return answers;
        }

        public BonusRoundAnswer GetBonusRoundAnswer(int bonusRoundNumber)
        {
            if (BonusRoundAnswers.Count <= bonusRoundNumber) return new BonusRoundAnswer(0, Question.NotJudged, bonusRoundNumber);
            else return BonusRoundAnswers[bonusRoundNumber];
        }

        public int GetScore()
        {
            UpdateScore();
            return Score;
        }

        public void UpdateScore()
        {
            if (ScoreNeedsUpdated)
            {
                int score = 0;
                var answers = AnswerSet.GetNonBonusAnswers();
                for (int i = 0; i < answers.Count; i++)
                {
                    for (int j = 0; j < answers[i].Count; j++)
                    {
                        if (answers[i][j] == Question.Correct) score += _pointsPerQuestion;
                        else if (answers[i][j] == Question.NotAnswered) score += (_pointsPerQuestion / 2);
                    }
                }
                foreach (var bonus in BonusRoundAnswers)
                {
                    if (bonus.Answer == Question.Correct) score += bonus.Wager;
                    else if (bonus.Answer == Question.Incorrect) score -= bonus.Wager;
                }
                Score = score;
                ScoreNeedsUpdated = false;
            }
            else return;
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
