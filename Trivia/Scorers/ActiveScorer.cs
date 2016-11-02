using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia.Scoring;
using TriviaData.Models;
using System.Runtime.Serialization;

namespace Trivia.Scorers
{
    [DataContract()]
    public class ActiveScorer
    {
        [DataMember()]
        public Scorer Scorer { get; private set; }
        [DataMember()]
        public List<ScoringTeam> ScoringTeams { get; private set; }

        /// <summary>
        /// The default constructor for use in building a fresh ActiveScorer
        /// </summary>
        /// <param name="s"></param>
        public ActiveScorer(Scorer s)
        {
            Scorer = s;
            ScoringTeams = new List<ScoringTeam>();
            foreach (var t in s.Teams) ScoringTeams.Add(new ScoringTeam(t));
        }

        /// <summary>
        /// For use with serialization for already-initialized game sessions.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="scoringTeams"></param>
        public ActiveScorer(Scorer s, List<ScoringTeam> scoringTeams)
        {
            Scorer = s;
            ScoringTeams = scoringTeams;
        }
    }
}
