using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia.ScoringHelpers;

namespace Trivia.Scoring
{
    public class BonusScoringRoundMasterViewModel : BindableBase
    {
        private GameState _gs;

        private BonusRoundScorecardViewModel _currentScorecardViewModel;
        public BonusRoundScorecardViewModel CurrentScorecardViewModel
        {
            get { return _currentScorecardViewModel; }
            set { SetProperty(ref _currentScorecardViewModel, value); }
        }

        private int _currentBonusRoundIndex;
        public int CurrentBonusRoundIndex
        {
            get { return _currentBonusRoundIndex; }
            set { SetProperty(ref _currentBonusRoundIndex, value); }
        }

        private ActiveScorer _currentScorer;
        public ActiveScorer CurrentScorer
        {
            get { return _currentScorer; }
            set { SetProperty(ref _currentScorer, value); }
        }

        public BonusScoringRoundMasterViewModel()
        {
            CurrentBonusRoundIndex = 0;
            CurrentScorecardViewModel = new BonusRoundScorecardViewModel();

            NextScorecardCommand = new RelayCommand(OnNextScorecard, NextScorecardExists);
            PreviousScorecardCommand = new RelayCommand(OnPrevScorecard, PrevScorecardExists);
            CancelAndReturnCommand = new RelayCommand(OnCancel);
            SaveAndReturnCommand = new RelayCommand(OnSaveAndReturn);
        }

        public void SetGameStateAndRoundNumber(RoundScoringParams roundParams)
        {
            _gs = roundParams.GameState;
            CurrentScorer = _gs.ActiveScorers[0];
            CurrentBonusRoundIndex = roundParams.RoundNumber;
            CurrentScorecardViewModel.SetRoundAndScorer(roundParams, CurrentScorer);
            CurrentScorecardViewModel.NextScorerRequested += HandleNextScorecardRequest;
            NextScorecardCommand.RaiseCanExecuteChanged();
            PreviousScorecardCommand.RaiseCanExecuteChanged();
        }

        private void HandleNextScorecardRequest(GameState gs)
        {
            _gs = gs;
            if (NextScorecardExists())
            {
                OnNextScorecard();
            }
            else
            {
                OnSaveAndReturn();
            }
        }

        private bool NextScorecardExists()
        {
            return CurrentScorer != _gs.ActiveScorers[_gs.ActiveScorers.Count - 1];
        }

        private void OnNextScorecard()
        {
            CurrentScorecardViewModel.OnSaveChanges();
            CurrentScorecardViewModel.NextScorerRequested -= HandleNextScorecardRequest;
            CurrentScorecardViewModel = new BonusRoundScorecardViewModel();
            CurrentScorer = _gs.ActiveScorers[_gs.ActiveScorers.IndexOf(CurrentScorer) + 1];
            CurrentScorecardViewModel.SetRoundAndScorer(new RoundScoringParams(_gs, CurrentBonusRoundIndex), CurrentScorer);
            CurrentScorecardViewModel.NextScorerRequested += HandleNextScorecardRequest;
            NextScorecardCommand.RaiseCanExecuteChanged();
            PreviousScorecardCommand.RaiseCanExecuteChanged();
        }

        private bool PrevScorecardExists()
        {
            return CurrentScorer != _gs.ActiveScorers[0];
        }

        private void OnPrevScorecard()
        {
            CurrentScorecardViewModel.OnSaveChanges();
            CurrentScorecardViewModel.NextScorerRequested -= HandleNextScorecardRequest;
            CurrentScorecardViewModel = new BonusRoundScorecardViewModel();
            CurrentScorer = _gs.ActiveScorers[_gs.ActiveScorers.IndexOf(CurrentScorer) - 1];
            CurrentScorecardViewModel.SetRoundAndScorer(new RoundScoringParams(_gs, CurrentBonusRoundIndex), CurrentScorer);
            CurrentScorecardViewModel.NextScorerRequested += HandleNextScorecardRequest;
            NextScorecardCommand.RaiseCanExecuteChanged();
            PreviousScorecardCommand.RaiseCanExecuteChanged();
        }

        private void OnCancel()
        {
            CurrentScorecardViewModel.NextScorerRequested -= HandleNextScorecardRequest;
            RoundCanceled();
        }

        private void OnSaveAndReturn()
        {
            CurrentScorecardViewModel.OnSaveChanges();
            CurrentScorecardViewModel.NextScorerRequested -= HandleNextScorecardRequest;
            BonusRoundComplete(_gs);
        }

        public RelayCommand NextScorecardCommand { get; private set; }
        public RelayCommand PreviousScorecardCommand { get; private set; }
        public RelayCommand CancelAndReturnCommand { get; private set; }
        public RelayCommand SaveAndReturnCommand { get; private set; }

        public event Action RoundCanceled = delegate { };
        public event Action<GameState> BonusRoundComplete = delegate { };
    }
}
