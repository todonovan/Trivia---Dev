using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia.Sessions;
using System.ComponentModel;
using Trivia.Scoreboard;
using Microsoft.Practices.Unity;

namespace Trivia.Scoring
{
    public class ScoringWindowViewModel : BindableBase
    {
        private ScoringMasterViewModel _scoringMasterViewModel;
        private ScorerRoundScorecardViewModel _scorerRoundScorecardViewModel;
        private ScoreboardWindowViewModel _scoreboardWindowViewModel;

        private GameSession _currentSession;
        public GameSession CurrentSession
        {
            get { return _currentSession; }
            set { SetProperty(ref _currentSession, value); }
        }

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

            CurrentViewModel = _scoringMasterViewModel;
        }

        public void SetCurrentGameSession(SessionConfigParams scp)
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;
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
        }


        // Need view models for...
        // Scoreboard
        // ScoringOverview
        // ScorerRoundScorecardView
        // maybe a details view? or a 'stats' view? 
    }
}
