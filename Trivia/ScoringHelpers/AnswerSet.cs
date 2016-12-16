using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivia.ScoringHelpers
{
    public enum Question { NotJudged, NotAnswered, Correct, Incorrect };

    public class AnswerSet
    {
        private List<List<Question>> _answers;

        public AnswerSet(int numRounds, int numQuestions)
        {
            _answers = new List<List<Question>>();
            for (int i = 0; i < numRounds; i++)
            {
                var roundList = new List<Question>(numQuestions);
                for (int j = 0; j < roundList.Capacity; j++)
                {
                    roundList.Add(Question.NotJudged);
                }
                _answers.Add(roundList);
            }
        }

        public AnswerSet(List<List<Question>> answers)
        {
            _answers = answers;
        }

        public Question GetAnswer(int round, int questionNum)
        {
            return _answers[round][questionNum];
        }

        public void SetAnswer(int round, int questionNum, Question val)
        {
            _answers[round][questionNum] = val;
        }

        public List<List<Question>> GetNonBonusAnswers()
        {
            List<List<Question>> allAnswers = new List<List<Question>>(_answers.Count);
            for (int i = 0; i < _answers.Count; i++)
            {
                allAnswers.Add(GetAnswersForRound(i));
            }
            return allAnswers;
        }

        public List<Question> GetAnswersForRound(int roundNumber)
        {
            List<Question> roundAnswers = new List<Question>(_answers[roundNumber].Count);
            for (int i = 0; i < roundAnswers.Capacity; i++) roundAnswers.Add(_answers[roundNumber][i]);
            return roundAnswers;
        }

    }
}
