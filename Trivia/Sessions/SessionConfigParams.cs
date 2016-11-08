using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia.Scorers;
using Trivia.Scoring;
using TriviaData.Models;

namespace Trivia.Sessions
{
    public class SessionConfigParams
    {
        public ObservableCollection<ActiveScorer> ActiveScorers { get; set; }
        public ObservableCollection<ScoringRound> ScoringRounds { get; set; }
        public int PointValue { get; private set; }
        public int NumRounds { get; private set; }

        public SessionConfigParams(ObservableCollection<Scorer> scorers, int numRounds, int pointValue)
        {
            ActiveScorers = new ObservableCollection<ActiveScorer>();
            foreach (var s in scorers) ActiveScorers.Add(new ActiveScorer(s));

            ScoringRounds = new ObservableCollection<ScoringRound>();
            for (int i = 0; i < numRounds - 1; i++)
            {
                ScoringRound sr = new ScoringRound(i, false, 5, pointValue);
                ScoringRounds.Add(sr);
            }
            ScoringRounds.Add(new ScoringRound(numRounds-1, true, 1, 0));
            PointValue = pointValue;
            NumRounds = numRounds;
        }
    }
}
