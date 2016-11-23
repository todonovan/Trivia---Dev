using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia.Scorers;
using Trivia.ScoringHelpers;
using Trivia.Sessions;

namespace Trivia.Scoring
{
    public class ScoringRoundMasterViewModel : BindableBase
    {
        private GameState _gs;

        private ScorerRoundScorecardViewModel _currentScorecardViewModel;
        public ScorerRoundScorecardViewModel CurrentScorecardViewModel
        {
            get { return _currentScorecardViewModel; }
            set { SetProperty(ref _currentScorecardViewModel, value); }
        }

        private int _currentRoundIndex;
        public int CurrentRoundIndex
        {
            get { return _currentRoundIndex; }
            set
            {
                SetProperty(ref _currentRoundIndex, value);
                NextScorecardCommand.RaiseCanExecuteChanged();
                PreviousScorecardCommand.RaiseCanExecuteChanged();
            }
        }

        private int _currentScorerNum;
        public int CurrentScorerNum
        {
            get { return _currentScorerNum; }
            set
            {
                SetProperty(ref _currentScorerNum, value);
                CurrentScorer = _gs.ActiveScorers[value];
            }
        }

        private ActiveScorer _currentScorer;
        public ActiveScorer CurrentScorer
        {
            get { return _currentScorer; }
            set { SetProperty(ref _currentScorer, value); }
        }

        public ScoringRoundMasterViewModel()
        {
            _currentScorerNum = 0;
            _currentScorecardViewModel = new ScorerRoundScorecardViewModel();
            NextScorecardCommand = new RelayCommand(OnNextScorecard, NextScorecardExists);
            PreviousScorecardCommand = new RelayCommand(OnPrevScorecard, PrevScorecardExists);
            ReturnToMasterViewCommand = new RelayCommand(OnReturnToMaster);
            FinishRoundCommand = new RelayCommand<GameState>(OnFinishRound);
        }

        public void SetGameStateAndRoundNumber(RoundScoringParams roundParams)
        {
            _gs = roundParams.GameState;
            CurrentScorer = _gs.ActiveScorers[0];
            CurrentRoundIndex = roundParams.RoundNumber;
            _currentScorecardViewModel.SetRoundAndScorer(roundParams, CurrentScorer);
        }

        private bool NextScorecardExists()
        {
            return _currentScorerNum + 1 < _gs.ActiveScorers.Count;
        }

        private void OnNextScorecard()
        {
            _currentScorerNum += 1;
            _currentScorecardViewModel.SetRoundAndScorer(new RoundScoringParams(_gs, CurrentRoundIndex), CurrentScorer);
        }

        private bool PrevScorecardExists()
        {
            return _currentScorerNum != 0;
        }

        private void OnPrevScorecard()
        {
            _currentScorerNum -= 1;
            _currentScorecardViewModel.SetRoundAndScorer(new RoundScoringParams(_gs, CurrentRoundIndex), CurrentScorer);
        }

        private void OnReturnToMaster()
        {
            RoundCanceled();
        }

        private void OnFinishRound(GameState gs)
        {
            RoundComplete(gs);
        }

        public RelayCommand NextScorecardCommand { get; private set; }
        public RelayCommand PreviousScorecardCommand { get; private set; }
        public RelayCommand ReturnToMasterViewCommand { get; private set; }
        public RelayCommand<GameState> FinishRoundCommand { get; private set; }

        public event Action RoundCanceled = delegate { };
        public event Action<GameState> RoundComplete = delegate { };
    }
}
