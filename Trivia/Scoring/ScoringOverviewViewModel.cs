using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia.ScoringHelpers;

namespace Trivia.Scoring
{
    public class ScoringOverviewViewModel : BindableBase
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

        private int _numberOfCompleteBonusRounds;
        public int NumberOfCompleteBonusRounds
        {
            get { return _numberOfCompleteBonusRounds; }
            set
            {
                SetProperty(ref _numberOfCompleteBonusRounds, value);
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

        private ObservableCollection<int> _bonusRoundIndices;
        public ObservableCollection<int> BonusRoundIndices
        {
            get { return _bonusRoundIndices; }
            set { SetProperty(ref _bonusRoundIndices, value); }
        }

        private int _selectedBonusRoundIndex;
        public int SelectedBonusRoundIndex
        {
            get { return _selectedBonusRoundIndex; }
            set { SetProperty(ref _selectedBonusRoundIndex, value - 1); }
        }

        private bool _tieExists;
        public bool TieExists
        {
            get { return _tieExists; }
            set { SetProperty(ref _tieExists, value); }
        }

        private ObservableCollection<string> _teamsTied;
        public ObservableCollection<string> TeamsTied
        {
            get { return _teamsTied; }
            set { SetProperty(ref _teamsTied, value); }
        }

        private int _topScore;

        public ScoringOverviewViewModel()
        {
            GoToRoundCommand = new RelayCommand(OnGoToRound);
            AutoScoreNextRoundCommand = new RelayCommand(OnAutoScoreNextRound, CanAutoScoreNextRound);
            ScoreNewBonusRoundCommand = new RelayCommand(OnScoreNewBonusRound, CanScoreNewBonusRound);
            FinishGameCommand = new RelayCommand(OnFinishGame, CanFinishGame);
        }

        public void SetGameState(GameState gs)
        {
            GameState = gs;
            PopulateRoundInfo();
            ScoreNewBonusRoundCommand.RaiseCanExecuteChanged();
            FinishGameCommand.RaiseCanExecuteChanged();
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
            List<int> bonusRoundIndices = new List<int>();
            for (int j = 1; j <= NumberOfCompleteBonusRounds; j++)
            {
                bonusRoundIndices.Add(j);
            }
            BonusRoundIndices = new ObservableCollection<int>(bonusRoundIndices);
            if (NumberOfRoundsScored == GameState.NumRounds) TieExists = DoesTieExist();
            else TieExists = false;
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
            NumberOfCompleteBonusRounds = GameState.NumberOfCompleteBonusRounds;
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

        private bool CanScoreNewBonusRound()
        {
            CheckNumberOfRoundsScored();
            return NumberOfRoundsScored == GameState.NumRounds && (NumberOfCompleteBonusRounds == 0 || DoesTieExist());
        }

        private void OnScoreNewBonusRound()
        {
            int bonusRoundIndex = NumberOfCompleteBonusRounds;
            GoToBonusRoundRequested(new RoundScoringParams(GameState, bonusRoundIndex));
        }

        private bool CanFinishGame()
        {
            return NumberOfRoundsScored == GameState.NumRounds && NumberOfCompleteBonusRounds > 0 && !DoesTieExist();
        }

        private void OnFinishGame()
        {
            FinishGameRequested(GameState);
        }

        private bool DoesTieExist()
        {
            var scores = GameState.GetAllScores().OrderBy(s => (-1 * s.Score)).ToList();
            _topScore = scores[0].Score;
            var tiedTeams = scores.TakeWhile(x => x.Score == _topScore).ToList();
            if (tiedTeams.Count == 1)
            {
                TeamsTied = new ObservableCollection<string>();
                return false;
            }
            else
            {
                TeamsTied = new ObservableCollection<string>();
                foreach (var t in tiedTeams) TeamsTied.Add(t.TeamName);
                return true;
            }
        }

        public RelayCommand GoToRoundCommand { get; private set; }
        public RelayCommand AutoScoreNextRoundCommand { get; private set; }
        public RelayCommand ScoreNewBonusRoundCommand { get; private set; }
        public RelayCommand FinishGameCommand { get; private set; }

        public event Action<RoundScoringParams> GoToRoundRequested = delegate { };
        public event Action<RoundScoringParams> GoToBonusRoundRequested = delegate { };
        public event Action<GameState> FinishGameRequested = delegate { };
    }
}
