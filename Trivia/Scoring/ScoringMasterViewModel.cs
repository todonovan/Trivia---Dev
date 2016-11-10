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

            BeginRoundCommand = new RelayCommand(OnBeginRound);
            TimerCommand = new RelayCommand(OnStartTimer);
            ScoreboardCommand = new RelayCommand(OnOpenScoreboard);
            ExitCommand = new RelayCommand(OnExit);
        }

        public void SetGameSession(GameSession gs)
        {
            _gameSession = gs;
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

        private void OnExit()
        {

        }

        private Dictionary<string, int> GetAllScores()
        {
            List<Dictionary<string, int>> scoresList = new List<Dictionary<string, int>>();
            foreach (var s in _gameSession.Scorers)
            {
                scoresList.Add(s.ReportScores());
            }

            return scoresList.SelectMany(dict => dict).ToDictionary(score => score.Key, score => score.Value);
        }

        public RelayCommand BeginRoundCommand { get; private set; }
        public RelayCommand TimerCommand { get; private set; }
        public RelayCommand ScoreboardCommand { get; private set; }
        public RelayCommand ExitCommand { get; private set; }
    }
}
