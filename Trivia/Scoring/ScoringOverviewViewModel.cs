using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia.ScoringHelpers;

namespace Trivia.Scoring
{
    public class ScoringOverviewViewModel: BindableBase
    {
        private GameState _gameState;
        public GameState GameState
        {
            get { return _gameState; }
            private set { SetProperty(ref _gameState, value); }
        }

        private int _numberOfRoundsScored;
        public int NumberOfRoundsScored
        {
            get { return _numberOfRoundsScored; }
            set
            {
                SetProperty(ref _numberOfRoundsScored, value);
                AutoScoreNextRoundCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<int> _gameRoundIndices;
        public ObservableCollection<int> GameRoundIndices
        {
            get { return _gameRoundIndices; }
            set { SetProperty(ref _gameRoundIndices, value); }
        }

        private int _selectedRoundIndex;
        public int SelectedRoundIndex
        {
            get { return _selectedRoundIndex; }
            set { SetProperty(ref _selectedRoundIndex, value - 1); }
        }

        public ScoringOverviewViewModel()
        {
            GoToRoundCommand = new RelayCommand(OnGoToRound);
            AutoScoreNextRoundCommand = new RelayCommand(OnAutoScoreNextRound, CanAutoScoreNextRound);
        }

        public void SetGameState(GameState gs)
        {
            GameState = gs;
            PopulateRoundInfo();
        }

        private void PopulateRoundInfo()
        {
            CheckNumberOfRoundsScored();
            List<int> roundIndices = new List<int>();
            for (int i = 1; i <= GameState.NumRounds; i++)
            {
                roundIndices.Add(i);
            }
            GameRoundIndices = new ObservableCollection<int>(roundIndices);
        }

        private void CheckNumberOfRoundsScored()
        {
            if (GameState == null) return;
            int count = 0;

            for (int i = 0; i < GameState.NumRounds; i++)
            {
                if (CheckIfRoundScored(i)) count++;
            }
            NumberOfRoundsScored = count;
        }

        private bool CheckIfRoundScored(int roundNumber)
        {
            foreach (var s in GameState.ActiveScorers)
            {
                if (!s.AllTeamsScoredForRound(roundNumber)) return false;
            }
            return true;
        }

        private void OnGoToRound()
        {
            GoToRoundRequested(new RoundScoringParams(GameState, SelectedRoundIndex));
        }

        private bool CanAutoScoreNextRound()
        {
            return NumberOfRoundsScored != GameState.NumRounds;
        }

        private void OnAutoScoreNextRound()
        {
            int roundToScoreIndex = 0;
            for (int i = 0; i < GameState.NumRounds; i++)
            {
                if (CheckIfRoundScored(i)) roundToScoreIndex++;
                else break;
            }
            GoToRoundRequested(new RoundScoringParams(GameState, roundToScoreIndex));
        }

        public RelayCommand GoToRoundCommand { get; private set; }
        public RelayCommand AutoScoreNextRoundCommand { get; private set; }

        public event Action<RoundScoringParams> GoToRoundRequested = delegate { };
    }
}
