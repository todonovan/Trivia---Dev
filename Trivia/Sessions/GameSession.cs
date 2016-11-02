using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia.Scorers;
using Trivia.Scoring;
using TriviaData.Models;
using System.Xml;
using System.Runtime.Serialization;

namespace Trivia.Sessions
{
    [DataContract()]
    public class GameSession
    {
        [DataMember()]
        public int NumRounds { get; set; }
        [DataMember()]
        public List<ScoringRound> Rounds { get; private set; }
        [DataMember()]
        public List<ActiveScorer> Scorers { get; private set; }

        /// <summary>
        /// The standard constructor for easy use when building a fresh, new GameSession
        /// </summary>
        /// <param name="numRounds"></param>
        /// <param name="numQuestionsPerRound"></param>
        /// <param name="pointsPerRound"></param>
        /// <param name="scorers">The constructor will construct fresh ActiveScorers.</param>
        public GameSession(int numRounds, int numQuestionsPerRound, List<int> pointsPerRound, List<Scorer> scorers)
        {
            NumRounds = numRounds;
            Rounds = new List<ScoringRound>();
            for (int i = 0; i < numRounds - 1; i++)
            {
                ScoringRound r = new ScoringRound(i, false, numQuestionsPerRound, pointsPerRound[i]);
                Rounds.Add(r);
            }
            Rounds.Add(new ScoringRound(numRounds - 1, true, pointsPerRound[numRounds-1], 0));  // add the bonus round with default params
        }
    }
}
