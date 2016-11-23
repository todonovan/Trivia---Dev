using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia.Scorers;
using Trivia.ScoringHelpers;

namespace Trivia.Scoring
{
    public class ScorerRoundScorecardViewModel : BindableBase
    {
        private GameState _gameState;

        private int _roundNumber;
        public int RoundNumber
        {
            get { return _roundNumber; }
            set { SetProperty(ref _roundNumber, value); }
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

        private ObservableCollection<TeamRoundScoringViewModel> _teamRoundScoringViewModels;
        public ObservableCollection<TeamRoundScoringViewModel> TeamRoundScoringViewModels
        {
            get { return _teamRoundScoringViewModels; }
            set { SetProperty(ref _teamRoundScoringViewModels, value); }
        }

        private TeamRoundScoringViewModel _selectedTeam;
        public TeamRoundScoringViewModel SelectedTeam
        {
            get { return _selectedTeam; }
            set { SetProperty(ref _selectedTeam, value); }
        }

        public ScorerRoundScorecardViewModel()
        {
            ScorerName = string.Empty;
            RoundNumber = 0;
        }

        public void SetRoundAndScorer(RoundScoringParams roundParams, ActiveScorer s)
        {
            RoundNumber = roundParams.RoundNumber + 1;
            _gameState = roundParams.GameState;
            Scorer = s;
            TeamRoundScoringViewModels = new ObservableCollection<TeamRoundScoringViewModel>();
            foreach (var t in s.ScoringTeams)
            {
                TeamRoundScoringViewModels.Add(new TeamRoundScoringViewModel());
            }
            ScorerName = s.Name;
        }
    }
}
