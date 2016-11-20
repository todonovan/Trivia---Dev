﻿using System;
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

namespace Trivia.Scoring
{
    public class ScoringWindowViewModel : BindableBase
    {
        private ScorerRoundScorecardViewModel _scorerRoundScorecardViewModel;
        private ScoreboardWindowViewModel _scoreboardWindowViewModel;
        private ScoringRoundMasterViewModel _scoringRoundMasterViewModel;

        private string _serializationName;

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
            _scoreboardWindowViewModel = ContainerHelper.Container.Resolve<ScoreboardWindowViewModel>();
            _scorerRoundScorecardViewModel = ContainerHelper.Container.Resolve<ScorerRoundScorecardViewModel>();
            _scoringRoundMasterViewModel = ContainerHelper.Container.Resolve<ScoringRoundMasterViewModel>();

            _scoringRoundMasterViewModel.RoundCanceled += OnRoundCanceled;
            _scoringRoundMasterViewModel.RoundComplete += OnRoundComplete;

            CurrentViewModel = ;

            BeginRoundCommand = new RelayCommand(OnBeginRound);
            TimerCommand = new RelayCommand(OnStartTimer);
            ScoreboardCommand = new RelayCommand(OnOpenScoreboard);
            ExitCommand = new RelayCommand(OnExit);
        }

        public void SetCurrentGameSession(SessionConfigParams scp)
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;
            GameState gs = GameStateFactory.GetNewGameState(scp);
            CurrentGameState = gs;
            
        }

        private void OnRoundComplete(GameState gs)
        {
            
        }

        private void OnBeginRound()
        {
            
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
            foreach (var s in CurrentGameState.ActiveScorers)
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
