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
                if (value < _gs.ActiveScorers.Count)
                {
                    SetProperty(ref _currentScorerNum, value);
                    CurrentScorer = _gs.ActiveScorers[value];
                }                
                NextScorecardCommand.RaiseCanExecuteChanged();
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
            CurrentScorecardViewModel = new ScorerRoundScorecardViewModel();
            NextScorecardCommand = new RelayCommand<GameState>(OnNextScorecard, NextScorecardExists);
            PreviousScorecardCommand = new RelayCommand(OnPrevScorecard, PrevScorecardExists);
            ReturnToMasterViewCommand = new RelayCommand(OnReturnToMaster);
            FinishRoundCommand = new RelayCommand(OnFinishRound);

            CurrentScorecardViewModel.NextScorerRequested += HandleNextScorecardRequest;
        }

        public void SetGameStateAndRoundNumber(RoundScoringParams roundParams)
        {
            _gs = roundParams.GameState;
            _currentScorerNum = 0;
            CurrentScorer = _gs.ActiveScorers[0];
            CurrentRoundIndex = roundParams.RoundNumber;
            CurrentScorecardViewModel.SetRoundAndScorer(roundParams, CurrentScorer);
            NextScorecardCommand.RaiseCanExecuteChanged();
            PreviousScorecardCommand.RaiseCanExecuteChanged();
        }

        private bool NextScorecardExists(GameState gs)
        {
            return CurrentScorer != _gs.ActiveScorers[_gs.ActiveScorers.Count - 1];
        }

        private void OnNextScorecard(GameState gs)
        {
            _gs = gs;
            CurrentScorecardViewModel.NextScorerRequested -= OnNextScorecard;
            CurrentScorecardViewModel = new ScorerRoundScorecardViewModel();
            CurrentScorerNum += 1;
            CurrentScorecardViewModel.SetRoundAndScorer(new RoundScoringParams(_gs, CurrentRoundIndex), CurrentScorer);
            CurrentScorecardViewModel.NextScorerRequested += HandleNextScorecardRequest;
        }

        private void HandleNextScorecardRequest(GameState gs)
        {
            _gs = gs;
            if (NextScorecardExists(_gs))
            {
                OnNextScorecard(_gs);
            }
            else
            {
                OnFinishRound();
            }
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

        private void OnFinishRound()
        {
            CurrentScorecardViewModel.NextScorerRequested -= OnNextScorecard;
            RoundComplete(_gs);
        }

        public RelayCommand<GameState> NextScorecardCommand { get; private set; }
        public RelayCommand PreviousScorecardCommand { get; private set; }
        public RelayCommand ReturnToMasterViewCommand { get; private set; }
        public RelayCommand FinishRoundCommand { get; private set; }

        public event Action RoundCanceled = delegate { };
        public event Action<GameState> RoundComplete = delegate { };
    }
}
