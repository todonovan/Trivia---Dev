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

namespace Trivia.Scoring
{
    public class ScoringWindowViewModel : BindableBase
    {
        private ScoringMasterViewModel _scoringMasterViewModel;
        private ScorerRoundScorecardViewModel _scorerRoundScorecardViewModel;
        private ScoreboardWindowViewModel _scoreboardWindowViewModel;
        private ScoringRoundMasterViewModel _scoringRoundMasterViewModel;

        private string _serializationName;

        private GameSession _currentSession;
        public GameSession CurrentSession
        {
            get { return _currentSession; }
            set { SetProperty(ref _currentSession, value); }
        }

        private ScoringRound _currentRound;
        public ScoringRound CurrentRound
        {
            get { return _currentRound; }
            set { SetProperty(ref _currentRound, value); }
        }

        private int _currentRoundIndex;

        private BindableBase _currentViewModel;
        public BindableBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }

        public ScoringWindowViewModel()
        {
            _scoringMasterViewModel = ContainerHelper.Container.Resolve<ScoringMasterViewModel>();
            _scoreboardWindowViewModel = ContainerHelper.Container.Resolve<ScoreboardWindowViewModel>();
            _scorerRoundScorecardViewModel = ContainerHelper.Container.Resolve<ScorerRoundScorecardViewModel>();
            _scoringRoundMasterViewModel = ContainerHelper.Container.Resolve<ScoringRoundMasterViewModel>();

            _scoringMasterViewModel.AutoScoreNextRoundRequested += AutoStartRoundScoring;

            _scoringRoundMasterViewModel.RoundCanceled += OnRoundCanceled;
            _scoringRoundMasterViewModel.RoundComplete += OnRoundComplete;

            CurrentRound = new ScoringRound(0, false, 5, 0);
            _currentRoundIndex = 0;

            CurrentViewModel = _scoringMasterViewModel;

            BeginRoundCommand = new RelayCommand(OnBeginRound);
            TimerCommand = new RelayCommand(OnStartTimer);
            ScoreboardCommand = new RelayCommand(OnOpenScoreboard);
            ExitCommand = new RelayCommand(OnExit);
        }

        public void SetCurrentGameSession(SessionConfigParams scp)
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;
            GameSession gs = new GameSession(scp.NumRounds, 5, scp.PointValue, scp.ActiveScorers.ToList());
            CurrentSession = gs;
            _scoringMasterViewModel.SetGameSession(CurrentSession);
        }

        /// <summary>
        /// For use with serialization.
        /// </summary>
        /// <param name="gs"></param>
        public void SetCurrentGameSession(GameSession gs)
        {
            CurrentSession = gs;
            CurrentRound = gs.Rounds[_currentRoundIndex];
        }

        private void AutoStartRoundScoring(int nextRound)
        {
            _scoringRoundMasterViewModel.SetGameSession(CurrentSession);
            _scoringRoundMasterViewModel.CurrentRound = CurrentSession.Rounds[nextRound - 1];
            CurrentViewModel = _scoringRoundMasterViewModel;
        }

        private void OnRoundCanceled()
        {
            CurrentViewModel = _scoringMasterViewModel;
        }

        private void OnRoundComplete(GameSession gs)
        {
            _currentRoundIndex += 1;
            SetCurrentGameSession(gs);
            GameSessionSerializer.SaveGameSession(gs, _serializationName);
        }

        private void OnBeginRound()
        {
            _scoringRoundMasterViewModel.SetGameSession(CurrentSession);
            _scoringRoundMasterViewModel.CurrentRound = CurrentRound;
            CurrentViewModel = _scoringRoundMasterViewModel;
        }

        private void OnStartTimer()
        {

        }

        private void OnOpenScoreboard()
        {
            Window w = new ScoreboardWindow();
            ScoreboardWindowViewModel vm = new ScoreboardWindowViewModel();
            Dictionary<string, int> currentScores = GetAllScores();
            vm.SetScores(currentScores);
            w.DataContext = vm;
            w.Show();
            vm.DisplayScores();
        }

        private Dictionary<string, int> GetAllScores()
        {
            List<Dictionary<string, int>> scoresList = new List<Dictionary<string, int>>();
            foreach (var s in CurrentSession.Scorers)
            {
                scoresList.Add(s.ReportScores());
            }

            return scoresList.SelectMany(dict => dict).ToDictionary(score => score.Key, score => score.Value);
        }

        private void OnExit()
        {

        }

        public RelayCommand BeginRoundCommand { get; private set; }
        public RelayCommand TimerCommand { get; private set; }
        public RelayCommand ScoreboardCommand { get; private set; }
        public RelayCommand ExitCommand { get; private set; }

        // Need view models for...
        // Scoreboard
        // ScoringOverview
        // ScorerRoundScorecardView
        // maybe a details view? or a 'stats' view? 
    }
}
