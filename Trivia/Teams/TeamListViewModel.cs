using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interactivity;
using Microsoft.Expression.Interactivity;
using System.Collections.ObjectModel;
using TriviaData.Models;
using TriviaData;
using TriviaData.Repos;

namespace Trivia.Teams
{
    public class TeamListViewModel : BindableBase
    {
        private ITeamRepository _repo;

        private ObservableCollection<Team> _teams;
        public ObservableCollection<Team> Teams
        {
            get { return _teams; }
            set { SetProperty(ref _teams, value); }
        }

        private List<Team> _teamsToDelete;

        private Team _selectedTeam;
        public Team SelectedTeam
        {
            get
            {
                return _selectedTeam;
            }
            set
            {
                _selectedTeam = value;
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }

        public TeamListViewModel(ITeamRepository repo)
        {
            _teamsToDelete = new List<Team>();
            DeleteCommand = new RelayCommand(OnDelete, CanDelete);
            UpdateDBCommand = new RelayCommand(OnUpdate, CanUpdate);
            EditTeamCommand = new RelayCommand<Team>(OnEditTeam);
            AddTeamCommand = new RelayCommand(OnAddTeam);
            _repo = repo;
        }

        private bool CanUpdate()
        {
            return _teamsToDelete.Count != 0;
        }

        private void OnUpdate()
        {
            foreach (var t in _teamsToDelete)
            {
                _repo.Remove(t);
            }
            _teamsToDelete = new List<Team>();
            UpdateDBCommand.RaiseCanExecuteChanged();
        }

        private bool CanDelete()
        {
            return SelectedTeam != null;
        }

        private void OnDelete()
        {
            _teamsToDelete.Add(SelectedTeam);
            Teams.Remove(SelectedTeam);
            UpdateDBCommand.RaiseCanExecuteChanged();
        }

        public void LoadTeams()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new System.Windows.DependencyObject())) return;
            Teams = new ObservableCollection<Team>(_repo.GetAllTeams());
            _teamsToDelete = new List<Team>();
            UpdateDBCommand.RaiseCanExecuteChanged();
        }

        public void OnAddTeam()
        {
            AddTeamRequested(new Team());
        }

        public void OnEditTeam(Team team)
        {
            EditTeamRequested(team);
        }

        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand UpdateDBCommand { get; private set; }
        public RelayCommand AddTeamCommand { get; private set; }
        public RelayCommand<Team> EditTeamCommand { get; private set; }

        public event Action<Team> AddTeamRequested = delegate { };
        public event Action<Team> EditTeamRequested = delegate { };
    }
}
