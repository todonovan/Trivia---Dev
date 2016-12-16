using System;
using System.Collections.Generic;
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

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set { SetProperty(ref _fileName, value); }
        }

        public LoadGameViewModel(IScorerRepository scorerRepo)
        {
            _scorerRepo = scorerRepo;
            _saveHandler = new GameStateSaveHandler(_scorerRepo);
            LoadGameCommand = new RelayCommand(OnLoadGame);
        }

        private void OnLoadGame()
        {
            GameState gs = _saveHandler.LoadGame(FileName);
            StartGameRequested(gs);
        }

        public RelayCommand LoadGameCommand { get; private set; }

        public Action<GameState> StartGameRequested = delegate { };
    }
}
