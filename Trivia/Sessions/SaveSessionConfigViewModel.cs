using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia.Scorers;
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
            get { return SessionConfigParams.NumberOfRounds; }
            private set { }
        }

        public int NumQuestions
        {
            get { return SessionConfigParams.NumberOfQuestions; }
            private set { }
        }

        public int PointsPerQuestion
        {
            get { return SessionConfigParams.PointsPerQuestion; }
            private set { }
        }

        public ObservableCollection<Scorer> Scorers
        {
            get { return new ObservableCollection<Scorer>(SessionConfigParams.Scorers); }
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
            SessionSerialization.SaveConfig(SessionConfigParams);
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
