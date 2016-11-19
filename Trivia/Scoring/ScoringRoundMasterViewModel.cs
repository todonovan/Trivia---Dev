using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia.Scorers;
using Trivia.Sessions;

namespace Trivia.Scoring
{
    public class ScoringRoundMasterViewModel : BindableBase
    {
        private GameSession _gs;

        private ScorerRoundScorecardViewModel _currentScorecardViewModel;
        public ScorerRoundScorecardViewModel CurrentScorecardViewModel
        {
            get { return _currentScorecardViewModel; }
            set { SetProperty(ref _currentScorecardViewModel, value); }
        }

        private ScoringRound _currentRound;
        public ScoringRound CurrentRound
        {
            get { return _currentRound; }
            set
            {
                SetProperty(ref _currentRound, value);
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
                CurrentScorer = _gs.Scorers[value];
            }
        }

        private ActiveScorer _currentScorer;
        public ActiveScorer CurrentScorer
        {
            get { return _currentScorer; }
            set
            {
                SetProperty(ref _currentScorer, value);
                CurrentScorecardViewModel.SetRoundAndScorer(CurrentRound, value);
            }
        }

        public ScoringRoundMasterViewModel()
        {
            _currentScorerNum = 0;
            _currentScorecardViewModel = new ScorerRoundScorecardViewModel();
            NextScorecardCommand = new RelayCommand(OnNextScorecard, NextScorecardExists);
            PreviousScorecardCommand = new RelayCommand(OnPrevScorecard, PrevScorecardExists);
            ReturnToMasterViewCommand = new RelayCommand(OnReturnToMaster);
            FinishRoundCommand = new RelayCommand<GameSession>(OnFinishRound);
        }

        public void SetGameSession(GameSession gs)
        {
            _gs = gs;
        }

        private bool NextScorecardExists()
        {
            return _currentScorerNum + 1 < _gs.Scorers.Count;
        }

        private void OnNextScorecard()
        {
            _currentScorerNum += 1;
            _currentScorecardViewModel.SetRoundAndScorer(CurrentRound, CurrentScorer);
        }

        private bool PrevScorecardExists()
        {
            return _currentScorerNum != 0;
        }

        private void OnPrevScorecard()
        {
            _currentScorerNum -= 1;
            _currentScorecardViewModel.SetRoundAndScorer(CurrentRound, CurrentScorer);
        }

        private void OnReturnToMaster()
        {
            RoundCanceled();
        }

        private void OnFinishRound(GameSession gs)
        {
            RoundComplete(gs);
        }

        public RelayCommand NextScorecardCommand { get; private set; }
        public RelayCommand PreviousScorecardCommand { get; private set; }
        public RelayCommand ReturnToMasterViewCommand { get; private set; }
        public RelayCommand<GameSession> FinishRoundCommand { get; private set; }

        public event Action RoundCanceled = delegate { };
        public event Action<GameSession> RoundComplete = delegate { };
    }
}
