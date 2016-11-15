using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaData.Models;
using TriviaData.Repos;
using System.Configuration;
using System.IO;
using System.ComponentModel;
using Trivia.Scorers;

namespace Trivia.Sessions
{
    public class LoadConfigViewModel : BindableBase
    {
        private IScorerRepository _scorerRepo;

        private List<string> _savedConfigNames;
        public List<string> SavedConfigNames
        {
            get { return _savedConfigNames; }
            set { SetProperty(ref _savedConfigNames, value); }
        }

        private string _selectedConfigName;
        public string SelectedConfigName
        {
            get { return _selectedConfigName; }
            set
            {
                SetProperty(ref _selectedConfigName, value);
                LoadCommand.RaiseCanExecuteChanged();
            }
        }

        private string _loadedConfigName;
        public string LoadedConfigName
        {
            get { return _loadedConfigName; }
            set { SetProperty(ref _loadedConfigName, value); }
        }

        private ObservableCollection<ActiveScorer> _loadedScorers;
        public ObservableCollection<ActiveScorer> LoadedScorers
        {
            get { return _loadedScorers; }
            set { SetProperty(ref _loadedScorers, value); }
        }

        private int _loadedNumTeams;
        public int LoadedNumTeams
        {
            get { return _loadedNumTeams; }
            set { SetProperty(ref _loadedNumTeams, value); }
        }

        private SessionConfigParams _loadedSession;

        public LoadConfigViewModel(IScorerRepository scorerRepo)
        {
            _scorerRepo = scorerRepo;
            CancelCommand = new RelayCommand(OnCancel);
            LoadCommand = new RelayCommand(OnLoad, CanLoad);
            UseConfigCommand = new RelayCommand(OnUseConfig, CanUseConfig);

            _loadedSession = null;
            SelectedConfigName = string.Empty;
            LoadedNumTeams = 0;
            LoadedConfigName = string.Empty;
            LoadedScorers = new ObservableCollection<ActiveScorer>();
        }

        public void PopulateFileList()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;
            string dirName = ConfigurationManager.AppSettings["session_config"].ToString();
            string[] rawConfigNames = Directory.GetFiles(dirName);
            List<string> configNames = new List<string>();
            foreach (var s in rawConfigNames)
            {
                string[] splitFileName = s.Split('\\');
                configNames.Add(splitFileName[splitFileName.Length - 1]);
            }
            SavedConfigNames = configNames;
        }

        private void OnCancel()
        {
            Done();
        }

        private bool CanLoad()
        {
            return SelectedConfigName != string.Empty;
        }

        private void OnLoad()
        {
            LoadedNumTeams = 0;
            _loadedSession = SessionConfig.LoadSession(_scorerRepo, SelectedConfigName);
            LoadedConfigName = SelectedConfigName;
            LoadedScorers = _loadedSession.ActiveScorers;
            foreach (var s in LoadedScorers) LoadedNumTeams += s.ScoringTeams.Count;
            UseConfigCommand.RaiseCanExecuteChanged();
        }

        private bool CanUseConfig()
        {
            return _loadedSession != null;
        }

        private void OnUseConfig()
        {
            UseConfigRequested(_loadedSession);
        }

        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand LoadCommand { get; private set; }
        public RelayCommand UseConfigCommand { get; private set; }

        public event Action<SessionConfigParams> UseConfigRequested = delegate { };
        public event Action Done = delegate { };
    }
}
