using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia.Sessions;
using System.ComponentModel;
using Trivia.Scoreboard;
using Microsoft.Practices.Unity;
using System.Windows;
using Trivia.ScoringHelpers;
using Trivia.GameSaving;
using Trivia.Reports;

namespace Trivia.Scoring
{
    public class ScoringWindowViewModel : BindableBase
    {
        private ScoringOverviewViewModel _scoringOverviewViewModel;
        private ScorerRoundScorecardViewModel _scorerRoundScorecardViewModel;
        private ScoringRoundMasterViewModel _scoringRoundMasterViewModel;
        private BonusScoringRoundMasterViewModel _bonusScoringRoundMasterViewModel;
        private GameStateSaveHandler _saveHandler;

        private Window _scoringWindow;

        private GameState _currentGameState;
        public GameState CurrentGameState
        {
            get { return _currentGameState; }
            set { SetProperty(ref _currentGameState, value); }
        }

        private BindableBase _currentViewModel;
        public BindableBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }

        public ScoringWindowViewModel()
        {
            _scoringOverviewViewModel = ContainerHelper.Container.Resolve<ScoringOverviewViewModel>();
            _scorerRoundScorecardViewModel = ContainerHelper.Container.Resolve<ScorerRoundScorecardViewModel>();
            _scoringRoundMasterViewModel = ContainerHelper.Container.Resolve<ScoringRoundMasterViewModel>();
            _bonusScoringRoundMasterViewModel = ContainerHelper.Container.Resolve<BonusScoringRoundMasterViewModel>();
            _saveHandler = ContainerHelper.Container.Resolve<GameStateSaveHandler>();

            CurrentViewModel = _scoringOverviewViewModel;

            _scoringOverviewViewModel.GoToRoundRequested += OnScoreRound;
            _scoringOverviewViewModel.GoToBonusRoundRequested += OnScoreBonusRound;
            _scoringOverviewViewModel.FinishGameRequested += OnFinishGame;
            _scoringRoundMasterViewModel.RoundComplete += OnRoundComplete;
            _scoringRoundMasterViewModel.RoundCanceled += OnRoundCanceled;
            _bonusScoringRoundMasterViewModel.RoundCanceled += OnRoundCanceled;
            _bonusScoringRoundMasterViewModel.BonusRoundComplete += OnBonusRoundComplete;

            ScoreboardCommand = new RelayCommand(OnOpenScoreboard);
            ExitCommand = new RelayCommand(OnExit, CanExit);
            StoreWindowCommand = new RelayCommand<Window>(OnStoreWindow);
        }

        public void SetCurrentGameState(GameState gs)
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;
            CurrentGameState = gs;
            _scoringOverviewViewModel.SetGameState(CurrentGameState);
        }

        private void OnOpenScoreboard()
        {
            Window w = new ScoreboardWindow();
            ScoreboardWindowViewModel vm = ContainerHelper.Container.Resolve<ScoreboardWindowViewModel>();
            List<ReportedScore> currentScores = CurrentGameState.GetAllScores();
            vm.SetScores(currentScores);
            w.DataContext = vm;
            w.Show();
        }

        private void OnStoreWindow(Window w)
        {
            _scoringWindow = w;
            ExitCommand.RaiseCanExecuteChanged();
        }

        private void OnExit()
        {
            if (_scoringWindow != null) _scoringWindow.Close();
        }

        private bool CanExit()
        {
            return _scoringWindow != null;
        }

        private void OnScoreRound(RoundScoringParams rsp)
        {
            _scoringRoundMasterViewModel.SetGameStateAndRoundNumber(rsp);
            CurrentViewModel = _scoringRoundMasterViewModel;
        }

        private void OnScoreBonusRound(RoundScoringParams rsp)
        {
            _bonusScoringRoundMasterViewModel.SetGameStateAndRoundNumber(rsp);
            CurrentViewModel = _bonusScoringRoundMasterViewModel;
        }

        private void OnRoundComplete(GameState gs)
        {
            _saveHandler.SaveGame(gs);
            SetCurrentGameState(gs);
            CurrentViewModel = _scoringOverviewViewModel;            
        }

        private void OnBonusRoundComplete(GameState gs)
        {
            gs.NumberOfCompleteBonusRounds += 1;
            _saveHandler.SaveGame(gs);
            SetCurrentGameState(gs);
            CurrentViewModel = _scoringOverviewViewModel;
        }

        private void OnRoundCanceled()
        {
            CurrentViewModel = _scoringOverviewViewModel;
        }

        private void OnFinishGame(GameState gs)
        {
            ReportFileHandler fileHandler = new ReportFileHandler();
            fileHandler.CreateReport(gs);
            if (_scoringWindow != null) _scoringWindow.Close();
        }

        public RelayCommand BeginRoundCommand { get; private set; }
        public RelayCommand ScoreboardCommand { get; private set; }
        public RelayCommand ExitCommand { get; private set; }
        public RelayCommand<Window> StoreWindowCommand { get; private set; }
    }
}
