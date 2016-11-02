using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaData.Models;
using System.Runtime.Serialization;

namespace Trivia.Scoring
{
    [DataContract()]
    public class ScoringTeam
    {
        [DataMember()]
        public Team Team { get; private set; }
        [DataMember()]
        public int Score { get; private set; }

        public ScoringTeam(Team t)
        {
            Team = t;
            Score = 0;
        }

        /// <summary>
        /// For use with serialization
        /// </summary>
        /// <param name="t"></param>
        /// <param name="score"></param>
        public ScoringTeam(Team t, int score)
        {
            Team = t;
            Score = score;
        }

        /// <summary>
        /// Currently a dumb wrapper to enforce setting of scores via explicit intentional method calls
        /// May be expanded later
        /// </summary>
        /// <param name="point value"></param>
        public void ChangeScore(int value)
        {
            Score += value;
        }
    }
}
