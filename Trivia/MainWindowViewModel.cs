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
using Trivia.ScoringHelpers;
using Trivia.GameSaving;
using Trivia.Reports;
using CsvHelper;

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
        private SaveSessionConfigViewModel _saveSessionConfigViewModel;
        private LoadConfigViewModel _loadConfigViewModel;
        private DeleteBadSessionViewModel _deleteBadSessionViewModel;
        private SessionStartConfirmViewModel _sessionStartConfirmViewModel;
        private LoadGameViewModel _loadGameViewModel;
        private ReportsListViewModel _reportsListViewModel;

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
            _saveSessionConfigViewModel = ContainerHelper.Container.Resolve<SaveSessionConfigViewModel>();
            _loadConfigViewModel = ContainerHelper.Container.Resolve<LoadConfigViewModel>();
            _deleteBadSessionViewModel = ContainerHelper.Container.Resolve<DeleteBadSessionViewModel>();
            _sessionStartConfirmViewModel = ContainerHelper.Container.Resolve<SessionStartConfirmViewModel>();
            _loadGameViewModel = ContainerHelper.Container.Resolve<LoadGameViewModel>();
            _reportsListViewModel = ContainerHelper.Container.Resolve<ReportsListViewModel>();
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

            _startSessionViewModel.Done += NavToLogin;
            _startSessionViewModel.LoadConfigRequested += NavToLoadConfig;
            _startSessionViewModel.LoadSavedSessionRequested += NavToLoadSavedSession;
            _startSessionViewModel.SaveConfigRequested += NavToSaveConfig;
            _startSessionViewModel.StartSessionRequested += NavToConfirmSession;

            _saveSessionConfigViewModel.Done += NavToLogin;

            _loadConfigViewModel.Done += NavToLogin;
            _loadConfigViewModel.UseConfigRequested += NavToConfirmSession;
            _loadConfigViewModel.FailedLoadError += NavToBadSession;

            _deleteBadSessionViewModel.Done += NavToLogin;

            _sessionStartConfirmViewModel.Done += NavToLogin;
            _sessionStartConfirmViewModel.StartSessionRequested += OpenScoringWindow;

            _loadGameViewModel.StartGameRequested += LoadScoringWindow;
            _loadGameViewModel.FailedLoadError += NavToBadSession;
            _loadGameViewModel.Done += NavToLogin;

            _reportsListViewModel.Done += NavToLogin;

            
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
                    NavToStartSession();
                    break;
                case "loadGame":
                    NavToLoadGame();
                    break;
                case "viewReport":
                    CurrentViewModel = _reportsListViewModel;
                    break;
                default:
                    CurrentViewModel = _loginViewModel;
                    break;
            }
        }

        private void NavToStartSession()
        {
            _startSessionViewModel.NumTeams = 0;
            _startSessionViewModel.UserNumRounds = 0;
            _startSessionViewModel.UserPointsPerQuestion = string.Empty;
            CurrentViewModel = _startSessionViewModel;
        }

        private void NavToBadSession(string sessionName)
        {
            _deleteBadSessionViewModel.SetSessionName(sessionName);
            CurrentViewModel = _deleteBadSessionViewModel;
        }

        private void NavToLoadGame()
        {
            CurrentViewModel = _loadGameViewModel;
        }

        private void NavToLogin()
        {
            CurrentViewModel = _loginViewModel;
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

        private void NavToLoadConfig()
        {
            CurrentViewModel = _loadConfigViewModel;
        }

        private void NavToLoadSavedSession()
        {

        }

        private void NavToSaveConfig(SessionConfigParams configParams)
        {
            _saveSessionConfigViewModel.SessionConfigParams = configParams;
            CurrentViewModel = _saveSessionConfigViewModel;
        }

        private void NavToConfirmSession(SessionConfigParams configParams)
        {
            _sessionStartConfirmViewModel.SetSessionConfig(configParams);
            CurrentViewModel = _sessionStartConfirmViewModel;
        }

        private void OpenScoringWindow(SessionConfigParams configParams)
        {
            Window w = new ScoringWindow();
            ScoringWindowViewModel vm = ContainerHelper.Container.Resolve<ScoringWindowViewModel>();
            GameState gs = GameStateFactory.GetNewGameState(configParams);
            vm.SetCurrentGameState(gs);
            w.DataContext = vm;
            w.Show();
            NavToStartSession();
        }

        private void LoadScoringWindow(GameState gs)
        {
            Window w = new ScoringWindow();
            ScoringWindowViewModel vm = ContainerHelper.Container.Resolve<ScoringWindowViewModel>();
            vm.SetCurrentGameState(gs);
            w.DataContext = vm;
            w.Show();
            NavToStartSession();
        }
    }
}
