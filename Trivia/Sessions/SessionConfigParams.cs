using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia.Scorers;
using Trivia.Scoring;
using Trivia.ScoringHelpers;
using TriviaData.Models;

namespace Trivia.Sessions
{
    public class SessionConfigParams
    {
        public int NumberOfRounds { get; set; }
        public int NumberOfQuestions { get; set; }
        public int PointsPerQuestion { get; set; }
        public List<Scorer> Scorers { get; set; }
        public string FileName { get; set; }

        public SessionConfigParams(int numRounds, int numQuestions, string userPointsPerRoundString, List<Scorer> scorers, string fileName)
        {
            NumberOfRounds = numRounds;
            NumberOfQuestions = numQuestions;
            PointsPerQuestion = int.Parse(userPointsPerRoundString);
            Scorers = scorers;
            FileName = fileName;
        }
    }
}
