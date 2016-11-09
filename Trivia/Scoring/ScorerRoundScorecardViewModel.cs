using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia.Scorers;

namespace Trivia.Scoring
{
    public class ScorerRoundScorecardViewModel : BindableBase
    {
        private ScoringRound _round;
        public int RoundNumber
        {
            get { return _round.OrderOfRound + 1; }
            private set { }
        }

        public int NumQuestions
        {
            get { return _round.NumQuestions; }
            private set { }
        }

        public ActiveScorer Scorer { get; private set; }
        public string ScorerName
        {
            get { return Scorer.Scorer.Name; }
            private set { }
        }

        private List<TeamRoundScoringViewModel> _teamRoundScoringViewModels;
        public List<TeamRoundScoringViewModel> TeamRoundScoringViewModels
        {
            get { return _teamRoundScoringViewModels; }
            set { SetProperty(ref _teamRoundScoringViewModels, value); }
        }

        public ScorerRoundScorecardViewModel()
        {

        }

        public void SetRoundAndScorer(ScoringRound r, ActiveScorer s)
        {
            _round = r;
            Scorer = s;
            TeamRoundScoringViewModels = new List<TeamRoundScoringViewModel>();
            foreach (var t in s.ScoringTeams)
            {
                TeamRoundScoringViewModels.Add(new TeamRoundScoringViewModel(NumQuestions));
            }
        }
    }
}
