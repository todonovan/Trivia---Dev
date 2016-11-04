using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia.Scoring;
using TriviaData.Models;
using TriviaData.Repos;

namespace Trivia.Sessions
{
    public class SaveSessionConfigViewModel : BindableBase
    {
        private IScorerRepository _scorerRepo;

        private SessionConfigParams _sessionConfigParams;
        public SessionConfigParams SessionConfigParams
        {
            get { return _sessionConfigParams; }
            set { SetProperty(ref _sessionConfigParams, value); }
        }

        public int NumRounds
        {
            get { return SessionConfigParams.ScoringRounds.Count; }
            private set { }
        }

        public ObservableCollection<ScoringRound> ScoringRounds
        {
            get { return SessionConfigParams.ScoringRounds; }
            private set { }
        }

        public ObservableCollection<Scorer> Scorers
        {
            get { return SessionConfigParams.Scorers; }
            private set { }
        }

        private string _configName;
        public string ConfigName
        {
            get { return _configName; }
            set
            {
                SetProperty(ref _configName, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public SaveSessionConfigViewModel(IScorerRepository repo)
        {
            _scorerRepo = repo;
            SessionConfigParams = new SessionConfigParams();
            SessionConfigParams.Scorers = new ObservableCollection<Scorer>();
            SessionConfigParams.ScoringRounds = new ObservableCollection<ScoringRound>();
            CancelCommand = new RelayCommand(OnCancel);
            SaveCommand = new RelayCommand(OnSave, CanSave);
        }

        private void OnCancel()
        {
            /// To do: change to gracefully return to session build screen.
            Done();
        }

        private bool CanSave()
        {
            return ConfigName != string.Empty;
        }

        private void OnSave()
        {
            SessionConfig.SaveConfig(SessionConfigParams, ConfigName);
            ConfigName = "Save successful!";
            System.Threading.Thread.Sleep(1000);
            Done();
        }

        public void SetSessionConfigParams(SessionConfigParams scp)
        {
            _sessionConfigParams = scp;
            ConfigName = string.Empty;
        }

        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }

        public event Action Done = delegate { };
    }
}
