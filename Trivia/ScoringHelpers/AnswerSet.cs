using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivia.ScoringHelpers
{
    public enum Question { NotAnswered, Correct, Incorrect };

    public class AnswerSet
    {
        private Question[,] _answers;

        public AnswerSet(int numRounds, int numQuestions)
        {
            _answers = new Question[numRounds, numQuestions];
        }

        public Question GetAnswer(int round, int questionNum)
        {
            return _answers[round, questionNum];
        }

        public void SetAnswer(int round, int questionNum, Question val)
        {
            _answers[round, questionNum] = val;
        }

        public List<List<Question>> GetAllAnswers()
        {
            List<List<Question>> answers = new List<List<Question>>();
            for (int i = 0; i < _answers.Rank; i++)
            {
                List<Question> roundAnswers = new List<Question>();
                for (int j = 0; j < _answers.GetLength(i); j++)
                {
                    roundAnswers.Add(_answers[i, j]);
                }
                answers.Add(roundAnswers);
            }
            return answers;
        }
    }
}
