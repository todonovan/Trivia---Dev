using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Trivia.Login;
using TriviaData;
using TriviaData.Models;
using Trivia.Teams;

namespace Trivia
{
    public class MainWindowViewModel : BindableBase
    {
        private TeamListViewModel _teamListViewModel;
        private TeamAddEditViewModel _teamAddEditViewModel;
        private LoginViewModel _loginViewModel;
        private TriviaDbContext _dbContext;

        private BindableBase _currentViewModel;
        private UserSession _currentUserSession;
        
        public MainWindowViewModel()
        {
            _dbContext = new TriviaDbContext();
            _dbContext.Open();
            _teamListViewModel = new TeamListViewModel(_dbContext);
            _teamAddEditViewModel = new TeamAddEditViewModel(_dbContext);
            _loginViewModel = new LoginViewModel();
            _currentViewModel = _loginViewModel;
            NavCommand = new RelayCommand<string>(OnNav);
            _teamListViewModel.AddTeamRequested += NavToAddTeam;
            _teamListViewModel.EditTeamRequested += NavToEditTeam;
            _teamAddEditViewModel.Done += NavToTeamList;
        }

        public BindableBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }

        private string _notificationMessage;
        public string NotificationMessage
        {
            get { return _notificationMessage; }
            set { SetProperty(ref _notificationMessage, value); }
        }

        public RelayCommand<string> NavCommand { get; private set; }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "teamAddEdit":
                    CurrentViewModel = _teamAddEditViewModel;
                    break;
                case "teamList":
                    CurrentViewModel = _teamListViewModel;
                    break;
                default:
                    CurrentViewModel = _loginViewModel;
                    break;
            }
        }

        private void NavToAddTeam(Team team)
        {
            _teamAddEditViewModel.EditMode = false;
            _teamAddEditViewModel.SetTeam(team);
            CurrentViewModel = _teamAddEditViewModel;
        }

        private void NavToEditTeam(Team team)
        {
            _teamAddEditViewModel.EditMode = true;
            _teamAddEditViewModel.SetTeam(team);
            CurrentViewModel = _teamAddEditViewModel;
        }

        private void NavToTeamList()
        {
            CurrentViewModel = _teamListViewModel;
        }
    }
}
