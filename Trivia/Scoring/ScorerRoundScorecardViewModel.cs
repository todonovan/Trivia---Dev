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
        public ScoringRound Round
        {
            get { return _round; }
            set
            {
                SetProperty(ref _round, value);
                RoundNumber = value.OrderOfRound + 1;
            }
        }

        public int RoundNumber { get; private set; }

        public int NumQuestions
        {
            get { return _round.NumQuestions; }
            private set { }
        }

        private ActiveScorer _scorer;
        public ActiveScorer Scorer
        {
            get { return _scorer; }
            set
            {
                SetProperty(ref _scorer, value);
                ScorerName = value.Name;
            }
        }

        public string ScorerName { get; private set; }

        private List<TeamRoundScoringViewModel> _teamRoundScoringViewModels;
        public List<TeamRoundScoringViewModel> TeamRoundScoringViewModels
        {
            get { return _teamRoundScoringViewModels; }
            set { SetProperty(ref _teamRoundScoringViewModels, value); }
        }

        public ScorerRoundScorecardViewModel()
        {
            ScorerName = string.Empty;
            RoundNumber = 0;
        }

        public void SetRoundAndScorer(ScoringRound r, ActiveScorer s)
        {
            _round = r;
            Scorer = s;
            TeamRoundScoringViewModels = new List<TeamRoundScoringViewModel>();
            foreach (var t in s.ScoringTeams)
            {
                TeamRoundScoringViewModels.Add(new TeamRoundScoringViewModel());
            }
            ScorerName = s.Name;
            RoundNumber = r.OrderOfRound + 1;
        }
    }
}
