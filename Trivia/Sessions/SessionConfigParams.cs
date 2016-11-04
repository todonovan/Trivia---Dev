using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia.Scoring;
using TriviaData.Models;

namespace Trivia.Sessions
{
    public class SessionConfigParams
    {
        public ObservableCollection<Scorer> Scorers { get; set; }
        public ObservableCollection<ScoringRound> ScoringRounds { get; set; }
    }
}
