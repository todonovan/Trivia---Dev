using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaData.Models;
using TriviaData.Repos;

namespace Trivia.Scorers
{
    public class ScorerSetTeamsViewModel : BindableBase
    {
        private ITeamRepository _teamRepo;
        private IScorerRepository _scorerRepo;

        private Scorer _scorerToAssociate;

        private ObservableCollection<Team> _teams;
        public ObservableCollection<Team> Teams
        {
            get { return _teams; }
            set { SetProperty(ref _teams, value); }
        }

        private ObservableCollection<Team> _allTeams;

        private ObservableCollection<Team> _teamsToAssociate;
        public ObservableCollection<Team> TeamsToAssociate
        {
            get { return _teamsToAssociate; }
            set
            {
                SetProperty(ref _teamsToAssociate, value);
                AssociateTeamsCommand.RaiseCanExecuteChanged();
            }
        }

        private Team _selectedTeam;
        public Team SelectedTeam
        {
            get { return _selectedTeam; }
            set
            {
                _selectedTeam = value;
                AddTeamCommand.RaiseCanExecuteChanged();
            }
        }

        private Team _selectedTeamToRemove;
        public Team SelectedTeamToRemove
        {
            get { return _selectedTeamToRemove; }
            set
            {
                _selectedTeamToRemove = value;
                RemoveTeamCommand.RaiseCanExecuteChanged();
            }
        }

        private List<Team> _teamsToRemove;

        private string _searchInput;
        public string SearchInput
        {
            get { return _searchInput; }
            set
            {
                SetProperty(ref _searchInput, value);
                FilterCustomers(_searchInput);
            }
        }

        private bool _filterHasScorer;
        public bool FilterHasScorer
        {
            get { return _filterHasScorer; }
            set
            {
                SetProperty(ref _filterHasScorer, value);
                FilterHasScorers();
            }
        }

        public ScorerSetTeamsViewModel(ITeamRepository teamRepo, IScorerRepository scorerRepo)
        {
            _teamRepo = teamRepo;
            _scorerRepo = scorerRepo;
            AssociateTeamsCommand = new RelayCommand(OnAssociateTeams);
            AddTeamCommand = new RelayCommand(OnAddTeam, CanAddTeam);
            RemoveTeamCommand = new RelayCommand(OnRemoveTeam, CanRemoveTeam);
            CancelCommand = new RelayCommand(OnCancel);
            ClearSearchCommand = new RelayCommand(OnClear);
        }               

        public void SetScorer(Scorer scorer)
        {
            _scorerToAssociate = scorer;
        }

        public void LoadTeams()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new System.Windows.DependencyObject())) return;
            Teams = new ObservableCollection<Team>(_teamRepo.GetAllTeams());
            var teamsToHandle = new List<Team>();
            foreach (var t in Teams)
            {
                if (_scorerToAssociate.Teams.Select(y => y.Id).Contains(t.Id))
                {
                    teamsToHandle.Add(t);
                }
            }
            foreach (var t in teamsToHandle)
            {
                Teams.Remove(t);
            }
            TeamsToAssociate = new ObservableCollection<Team>(_scorerToAssociate.Teams);
            _teamsToRemove = new List<Team>();
            _allTeams = Teams;
            FilterHasScorer = false;
        }

        private bool CanAssociateTeams()
        {
            return TeamsToAssociate.Count != 0 || _teamsToRemove.Count != 0;
        }

        private void OnAssociateTeams()
        {
            List<Team> teamsToAssociate = TeamsToAssociate.ToList();
            foreach (var t in teamsToAssociate)
            {
                if (!_scorerToAssociate.Teams.Select(x => x.Id).Contains(t.Id))
                {
                    _teamRepo.AddTeamToScorer(_scorerToAssociate, t);
                }                
            }
            foreach (var t in _teamsToRemove)
            {
                if (_scorerToAssociate.Teams.Select(x => x.Id).Contains(t.Id))
                {
                    _teamRepo.RemoveTeamFromScorer(_scorerToAssociate, t);
                }
            }
            _scorerToAssociate.Teams = teamsToAssociate;
            _scorerRepo.Update(_scorerToAssociate);
            AssociateTeamsCommand.RaiseCanExecuteChanged();
            Done();
        }

        private bool CanAddTeam()
        {
            return SelectedTeam != null;
        }

        private void OnAddTeam()
        {
            TeamsToAssociate.Add(SelectedTeam);
            Teams.Remove(SelectedTeam);
            SelectedTeam = null;
        }

        private bool CanRemoveTeam()
        {
            return SelectedTeamToRemove != null;
        }

        private void OnRemoveTeam()
        {
            Teams.Add(SelectedTeamToRemove);
            _allTeams.Add(SelectedTeamToRemove);
            _teamsToRemove.Add(SelectedTeamToRemove);
            TeamsToAssociate.Remove(SelectedTeamToRemove);
            SelectedTeamToRemove = null;
        }

        private void OnCancel()
        {
            Done();
        }

        private void OnClear()
        {
            SearchInput = null;
        }

        private void FilterCustomers(string searchInput)
        {
            if (string.IsNullOrWhiteSpace(searchInput))
            {
                Teams = new ObservableCollection<Team>(_allTeams);
            }
            else
            {
                Teams = new ObservableCollection<Team>(_allTeams.Where(c => c.Year.ToString().Contains(searchInput)));
            }
        }

        private void FilterHasScorers()
        {
            if (FilterHasScorer)
            {
                Teams = new ObservableCollection<Team>(_allTeams.Where(t => !t.HasScorer));
            }
            else
            {
                Teams = new ObservableCollection<Team>(_allTeams);
            }
        }

        public RelayCommand AssociateTeamsCommand { get; private set; }
        public RelayCommand AddTeamCommand { get; private set; }
        public RelayCommand RemoveTeamCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand ClearSearchCommand { get; private set; }

        public event Action Done = delegate { };
    }
}
