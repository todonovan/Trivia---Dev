using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia.ScoringHelpers;
using TriviaData.Repos;

namespace Trivia.GameSaving
{
    public class LoadGameViewModel : BindableBase
    {
        private IScorerRepository _scorerRepo;
        private GameStateSaveHandler _saveHandler;

        private ObservableCollection<string> _savedGameNames;
        public ObservableCollection<string> SavedGameNames
        {
            get { return _savedGameNames; }
            set { SetProperty(ref _savedGameNames, value); }
        }

        private string _selectedGameName;
        public string SelectedGameName
        {
            get { return _selectedGameName; }
            set
            {
                SetProperty(ref _selectedGameName, value);
                LoadGameCommand.RaiseCanExecuteChanged();
            }
        }

        public LoadGameViewModel(IScorerRepository scorerRepo)
        {
            _scorerRepo = scorerRepo;
            _saveHandler = new GameStateSaveHandler(_scorerRepo);
            LoadGameCommand = new RelayCommand(OnLoadGame, CanLoadGame);
            CancelCommand = new RelayCommand(OnCancel);
            SelectedGameName = string.Empty;
        }

        public void PopulateFileList()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;
            string dirName = ConfigurationManager.AppSettings["game_save_config"].ToString();
            string[] rawGameNames = Directory.GetFiles(dirName);
            List<string> gameNames = new List<string>();
            foreach (var s in rawGameNames)
            {
                string[] splitFileName = s.Split('\\');
                gameNames.Add(splitFileName[splitFileName.Length - 1]);
            }
            SavedGameNames = new ObservableCollection<string>(gameNames);
        }

        private bool CanLoadGame()
        {
            return SelectedGameName != string.Empty;
        }

        private void OnLoadGame()
        {
            GameState gs = _saveHandler.LoadGame(SelectedGameName);
            StartGameRequested(gs);
        }

        private void OnCancel()
        {
            Done();
        }

        public RelayCommand LoadGameCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        public Action<GameState> StartGameRequested = delegate { };
        public Action Done = delegate { };
    }
}
