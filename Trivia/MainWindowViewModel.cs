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
using TriviaData.Repos;
using Microsoft.Practices.Unity;
using Trivia.Scorers;
using Trivia.Scoring;
using Trivia.Sessions;
using System.Windows;

namespace Trivia
{
    public class MainWindowViewModel : BindableBase
    {
        private TeamListViewModel _teamListViewModel;
        private TeamAddEditViewModel _teamAddEditViewModel;
        private ScorerListViewModel _scorerListViewModel;
        private ScorerAddEditViewModel _scorerAddEditViewModel;
        private ScorerSetTeamsViewModel _scorerSetTeamsViewModel;
        private LoginViewModel _loginViewModel;
        private StartSessionViewModel _startSessionViewModel;

        private BindableBase _currentViewModel;
        private UserSession _currentUserSession;
        
        public MainWindowViewModel()
        {
            _teamListViewModel = ContainerHelper.Container.Resolve<TeamListViewModel>();
            _teamAddEditViewModel = ContainerHelper.Container.Resolve<TeamAddEditViewModel>();
            _scorerListViewModel = ContainerHelper.Container.Resolve<ScorerListViewModel>();
            _scorerAddEditViewModel = ContainerHelper.Container.Resolve<ScorerAddEditViewModel>();
            _scorerSetTeamsViewModel = ContainerHelper.Container.Resolve<ScorerSetTeamsViewModel>();
            _loginViewModel = ContainerHelper.Container.Resolve<LoginViewModel>();
            _startSessionViewModel = ContainerHelper.Container.Resolve<StartSessionViewModel>();
            _currentViewModel = _loginViewModel;

            NavCommand = new RelayCommand<string>(OnNav);

            _teamListViewModel.AddTeamRequested += NavToAddTeam;
            _teamListViewModel.EditTeamRequested += NavToEditTeam;
            _teamAddEditViewModel.Done += NavToTeamList;

            _scorerListViewModel.AddScorerRequested += NavToAddScorer;
            _scorerListViewModel.EditScorerRequested += NavToEditScorer;
            _scorerListViewModel.AssociateTeamsRequested += NavToAssociateTeamsWithScorer;
            _scorerAddEditViewModel.Done += NavToScorerList;
            _scorerAddEditViewModel.AssociateTeamsRequested += NavToAssociateTeamsWithScorer;
            _scorerSetTeamsViewModel.Done += NavToScorerList;
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
        public RelayCommand StartSessionCommand { get; private set; }

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
                case "scorerList":
                    CurrentViewModel = _scorerListViewModel;
                    break;
                case "startSession":
                    CurrentViewModel = _startSessionViewModel;
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

        private void NavToAddScorer(Scorer scorer)
        {
            _scorerAddEditViewModel.EditMode = false;
            _scorerAddEditViewModel.SetScorer(scorer);
            CurrentViewModel = _scorerAddEditViewModel;
        }

        private void NavToEditScorer(Scorer scorer)
        {
            _scorerAddEditViewModel.EditMode = true;
            _scorerAddEditViewModel.SetScorer(scorer);
            CurrentViewModel = _scorerAddEditViewModel;
        }

        private void NavToScorerList()
        {
            CurrentViewModel = _scorerListViewModel;
        }

        private void NavToAssociateTeamsWithScorer(Scorer scorer)
        {
            _scorerSetTeamsViewModel.SetScorer(scorer);
            CurrentViewModel = _scorerSetTeamsViewModel;
        }
    }
}
