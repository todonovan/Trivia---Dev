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

        private ObservableCollection<string> _savedConfigNames;
        public ObservableCollection<string> SavedConfigNames
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
                DeleteConfigCommand.RaiseCanExecuteChanged();
            }
        }

        private string _loadedConfigName;
        public string LoadedConfigName
        {
            get { return _loadedConfigName; }
            set { SetProperty(ref _loadedConfigName, value); }
        }

        private ObservableCollection<Scorer> _loadedScorers;
        public ObservableCollection<Scorer> LoadedScorers
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
            DeleteConfigCommand = new RelayCommand(OnDeleteConfig, CanDeleteConfig);
            _loadedSession = null;
            SelectedConfigName = string.Empty;
            LoadedNumTeams = 0;
            LoadedConfigName = string.Empty;
            LoadedScorers = new ObservableCollection<Scorer>();
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
            SavedConfigNames = new ObservableCollection<string>(configNames);
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
            try
            {
                _loadedSession = SessionSerialization.LoadSession(_scorerRepo, SelectedConfigName);
                LoadedConfigName = SelectedConfigName;
                LoadedScorers = new ObservableCollection<Scorer>(_loadedSession.Scorers);
                foreach (var s in LoadedScorers) LoadedNumTeams += s.Teams.Count;
                UseConfigCommand.RaiseCanExecuteChanged();
            }
            catch (InvalidCastException)
            {
                FailedLoadError(SelectedConfigName);
            }            
        }

        private bool CanUseConfig()
        {
            return _loadedSession != null;
        }

        private void OnUseConfig()
        {
            UseConfigRequested(_loadedSession);
        }

        private bool CanDeleteConfig()
        {
            return SelectedConfigName != string.Empty;
        }

        private void OnDeleteConfig()
        {
            var confirmResult = System.Windows.MessageBox.Show("Are you sure? This will cause any saved games using this config to become unusable.", "Confirm Delete", System.Windows.MessageBoxButton.OKCancel);
            if (confirmResult == System.Windows.MessageBoxResult.OK)
            {
                File.Delete(ConfigurationManager.AppSettings["session_config"].ToString() + SelectedConfigName);
                LoadedConfigName = string.Empty;
                PopulateFileList();
            }
            else return;
        }

        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand LoadCommand { get; private set; }
        public RelayCommand UseConfigCommand { get; private set; }
        public RelayCommand DeleteConfigCommand { get; private set; }

        public event Action<SessionConfigParams> UseConfigRequested = delegate { };
        public event Action<string> FailedLoadError = delegate { };
        public event Action Done = delegate { };
    }
}
