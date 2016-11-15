using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Trivia.Scoreboard;
using Trivia.Sessions;

namespace Trivia.Scoring
{
    public class ScoringMasterViewModel : BindableBase
    {
        private GameSession _gameSession;

        private int _currentRound;
        public int CurrentRound
        {
            get { return _currentRound; }
            set { SetProperty(ref _currentRound, value); }
        }

        public int NumRounds
        {
            get { return _gameSession.NumRounds; }
        }

        public ScoringMasterViewModel()
        {
            CurrentRound = 1;

          
        }

        public void SetGameSession(GameSession gs)
        {
            _gameSession = gs;
        }

        private void OnAutoScoreNextRound()
        {
            AutoScoreNextRoundRequested(CurrentRound);
        }        

        public RelayCommand AutoScoreNextRoundCommand { get; private set; }


        public event Action<int> AutoScoreNextRoundRequested = delegate { };
    }
}
