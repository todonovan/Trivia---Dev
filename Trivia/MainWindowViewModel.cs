using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Trivia.Teams;
using TriviaData;

namespace Trivia
{
    public class MainWindowViewModel : BindableBase
    {
        private TeamListViewModel _teamListViewModel;
        private TeamAddViewModel _teamAddViewModel;
        private TeamEditViewModel _teamEditViewModel;
        private TriviaDbContext _dbContext;

        private BindableBase _currentViewModel;
        
        public MainWindowViewModel()
        {
            _dbContext = new TriviaDbContext();
            _dbContext.Open();
            _teamListViewModel = new TeamListViewModel(_dbContext);
            _teamAddViewModel = new TeamAddViewModel(_dbContext);
            _teamEditViewModel = new TeamEditViewModel(_dbContext);
            _currentViewModel = _teamListViewModel;
            NavCommand = new RelayCommand<string>(OnNav);
        }

        public BindableBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }

        public RelayCommand<string> NavCommand { get; private set; }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "teamAdd":
                    CurrentViewModel = _teamAddViewModel;
                    break;
                case "teamEdit":
                    CurrentViewModel = _teamEditViewModel;
                    break;
                case "teamList":
                default:
                    CurrentViewModel = _teamListViewModel;
                    break;
            }
        }
    }
}
